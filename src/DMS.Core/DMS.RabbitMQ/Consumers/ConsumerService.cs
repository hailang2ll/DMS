using DMS.BaseFramework.Common.Extension;
using DMS.Log4net;
using DMS.RabbitMQ.Context;
using DMS.RabbitMQ.Extensions;
using DMS.RabbitMQ.Options;
using DMS.RabbitMQ.Utils;
using log4net;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;

namespace DMS.RabbitMQ.Consumers
{
    /// <summary>
    /// 基础消费者
    /// </summary>
    public class ConsumerService
    {
        private readonly IModel _channel;
        private readonly List<int> _retryTime;
        private readonly string _queueName;
        public const string RetryCountKeyName = "RetryCount";
        public ConsumerService(string queueName)
        {
            _channel = RabbitMQContext.GetModel(queueName);
            _queueName = queueName;

            _retryTime = new List<int>
            {
                1 * 1000,
                10 * 1000,
                30 * 1000
            };
        }

        /// <summary>
        /// 拉取指定队列的消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="autoAck">是否自动应答（true表示自动应答；false则需要手动应答）</param>
        /// <returns></returns>
        public void Pull<T>(Action<T> handler) where T : class
        {
            BasicGetResult result = _channel.BasicGet(_queueName, false);
            if (result != null)
            {
                var body = result.Body;
                var msgStr = body.StreamToStr();
                var msg = SerializerJson.DeserializeObject<T>(msgStr);
                string messageId = result.BasicProperties.MessageId;//消息Id
                try
                {
                    handler(msg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"队列：{_queueName}，MessageId：{messageId}", ex);
                }
                finally
                {
                    _channel.BasicAck(result.DeliveryTag, false);
                }
            }

        }


        /// <summary>
        /// 接收消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue">队列名称</param>
        /// <param name="isProperties"></param>
        /// <param name="handler">消费处理</param>
        /// <param name="isDeadLetter"></param>
        public void Subscribe<T>(Action<T> handler) where T : class
        {
            //每次消费的消息数
            _channel.BasicQos(0, 1, false);

            RabbitMessageQueueOptions queueOptions = RabbitQueueOptionsUtil.ReaderByQueue(_queueName);

            _channel.ExchangeDeclare(queueOptions.ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(_queueName, true, false, false, null);
            _channel.QueueBind(_queueName, queueOptions.ExchangeName, queueOptions.RoutingKeys);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var msgStr = body.StreamToStr();
                var msg = SerializerJson.DeserializeObject<T>(msgStr);
                string messageId = ea.BasicProperties.MessageId;//消息Id
                try
                {
                    handler(msg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"队列：{_queueName}，MessageId：{messageId}", ex);
                }
                finally
                {
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            _channel.BasicConsume(_queueName, false, consumer);
        }


        /// <summary>
        /// 接收消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue">队列名称</param>
        /// <param name="isProperties"></param>
        /// <param name="handler">消费处理</param>
        /// <param name="isDeadLetter"></param>
        public void SubscribeRetry<T>(Action<T> handler) where T : class
        {

            //每次消费的消息数
            _channel.BasicQos(0, 1, false);

            RabbitMessageQueueOptions queueOptions = RabbitQueueOptionsUtil.ReaderByQueue(_queueName);

            _channel.ExchangeDeclare(queueOptions.ExchangeName, queueOptions.ExchangeType, queueOptions.Persistent);
            _channel.QueueDeclare(_queueName, true, false, false, null);
            _channel.QueueBind(_queueName, queueOptions.ExchangeName, queueOptions.RoutingKeys);

            var retryDic = new Dictionary<string, object>
            {
                {"x-dead-letter-exchange", queueOptions.ExchangeName},
                {"x-dead-letter-routing-key", queueOptions.RoutingKeys}
            };
            _channel.ExchangeDeclare(queueOptions.ExchangeName.GetRetrySuffixName(), queueOptions.ExchangeType);
            _channel.QueueDeclare(_queueName.GetRetrySuffixName(), true, false, false, retryDic);
            _channel.QueueBind(_queueName.GetRetrySuffixName(), queueOptions.ExchangeName.GetRetrySuffixName(), queueOptions.RoutingKeys.GetRetrySuffixName());


            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                bool canAck = false;//应答结果
                int retryCount = 0;//第几次重试
                string messageId = ea.BasicProperties.MessageId;
                IDictionary<string, object> headers = ea.BasicProperties.Headers;
                if (headers != null && headers.ContainsKey(RetryCountKeyName))
                {
                    retryCount = (int)headers[RetryCountKeyName]; retryCount++;
                    Console.WriteLine($"队列：{_queueName}，MessageId：{messageId}，第：{retryCount}次重试开始！！！");
                    Logger.Info($"队列：{_queueName}，MessageId：{messageId}，第：{retryCount}次重试开始！！！");
                }

                try
                {
                    var body = ea.Body;
                    var msgStr = body.StreamToStr();
                    var msg = SerializerJson.DeserializeObject<T>(msgStr);

                    handler(msg);
                    canAck = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"队列：{_queueName}，MessageId：{messageId}，第：{retryCount}次消费发生异常：", ex);
                    Logger.Error($"队列：{_queueName}，MessageId：{messageId}，第：{retryCount}次消费发生异常：", ex);
                    if (CanRetry(retryCount))
                    {
                        RetryHandler(retryCount, queueOptions.ExchangeName.GetRetrySuffixName(), queueOptions.RoutingKeys.GetRetrySuffixName(), ea);
                        canAck = true;
                    }
                    else
                    {
                        canAck = false;
                    }
                }

                //处理应答
                AnswerHandler(canAck, ea);
            };

            _channel.BasicConsume(_queueName, false, consumer);
        }


        /// <summary>
        /// 应答处理
        /// </summary>
        /// <param name="canAck"></param>
        /// <param name="eventArgs"></param>
        /// <returns>应答处理结果，true：表示成功，false：表示失败</returns>
        protected virtual void AnswerHandler(bool canAck, BasicDeliverEventArgs eventArgs)
        {
            try
            {
                ulong deliveryTag = eventArgs.DeliveryTag;
                if (canAck)
                {
                    _channel.BasicAck(deliveryTag, false);
                }
                else
                {
                    _channel.BasicNack(deliveryTag, false, false);
                }
            }
            catch (AlreadyClosedException ex)
            {
                string messageId = eventArgs.BasicProperties.MessageId;
                Console.WriteLine($"MessageId：{messageId}，重试发生异常(RabbitMQ is Closed)：", ex);
                Logger.Info($"MessageId：{messageId}，重试发生异常(RabbitMQ is Closed)：", ex);
            }
        }

        /// <summary>
        /// 重试次数
        /// </summary>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        private bool CanRetry(int retryCount)
        {
            return retryCount <= _retryTime.Count - 1;
        }

        /// <summary>
        /// 重试管道处理
        /// </summary>
        /// <param name="retryCount"></param>
        /// <param name="retryExchange"></param>
        /// <param name="retryRoute"></param>
        /// <param name="ea"></param>
        private void RetryHandler(int retryCount, string retryExchange, string retryRoute, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var properties = ea.BasicProperties;
            properties.Headers = properties.Headers ?? new Dictionary<string, object>();
            properties.Headers[RetryCountKeyName] = retryCount;
            properties.Expiration = _retryTime[retryCount].ToString();

            try
            {
                _channel.BasicPublish(retryExchange, retryRoute, properties, body);
            }
            catch (AlreadyClosedException ex)
            {
                Console.WriteLine($"MessageId：{ea.BasicProperties.MessageId}重试发生异常(RabbitMQ is Closed)：", ex);
                Logger.Info($"MessageId：{ea.BasicProperties.MessageId}重试发生异常(RabbitMQ is Closed)：", ex);
            }
        }
    }
}

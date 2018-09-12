using DMS.Log4net;
using DMS.RabbitMQ.Context;
using DMS.RabbitMQ.Extensions;
using DMS.RabbitMQ.Interface;
using DMS.RabbitMQ.Models;
using DMS.RabbitMQ.Options;
using DMS.RabbitMQ.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS.RabbitMQ.Consumers
{
    /// <summary>
    /// 基础消费者
    /// </summary>
    class BaseConsumer
    {
        /// <summary>
        /// 消息连接通道 
        /// </summary>
        protected IModel _channel;
        /// <summary>
        /// 重试队列名称
        /// </summary>
        protected string _retryQueueName;
        /// <summary>
        /// 配置文件
        /// </summary>
        protected RabbitmqServiceOptions _config;
        /// <summary>
        /// 重试的时间规则
        /// </summary>
        protected List<int> _retryTimeRules;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pluginPath"></param>
        /// <param name="channel"></param>
        /// <param name="config"></param>
        public BaseConsumer(IModel channel, RabbitmqServiceOptions config)
        {
            _config = config;
            _channel = channel;
            _retryQueueName = _config.QueueName.GetTaskRoutingKey(); ;//重试队列名称
            config.ExchangeName = config.PatternType.GetExchangeName(config.ExchangeName);//交换机名称
            _retryTimeRules = config?.RetryTimeRules?.Select(p => p * 1000).OrderBy(p => p).ToList();//加载重试的时间规则
        }

        /// <summary>
        /// 启动
        /// BasicQos：设置同一时刻服务器只会发送一条消息给消费者
        /// </summary>
        /// <param name="connectionStrings">连接字符串</param>
        /// <param name="serviceConfiguration"></param>
        public virtual void Start()
        {
            _channel.BasicQos(0, 1, false);
            var arguments = new Dictionary<string, object>
            {
                {"x-dead-letter-exchange",_config.ExchangeName},
                {"x-dead-letter-routing-key", _config.QueueName}
            };
            _channel.QueueDeclare(_retryQueueName, true, false, false, arguments);
            _channel.ExchangeDeclare(RabbitMQContext.TaskExchangeName, "direct");
            _channel.QueueBind(_retryQueueName, RabbitMQContext.TaskExchangeName, _retryQueueName);

            var exchangeType = _config.PatternType.GetExchangeType();
            _channel.ExchangeDeclare(_config.ExchangeName, exchangeType);//声明交换机
            _channel.QueueDeclare(_config.QueueName, _config.Durable, false, false, arguments: null);//声明队列
            _channel.QueueBind(_config.QueueName, _config.ExchangeName, _config.QueueName);//建立队列与交换机的绑定关系

            var consumer = new EventingBasicConsumer(_channel);//定义消息接收回调
            consumer.Received += ReceivedHandler;//回调接收处理
            _channel.BasicConsume(_config.QueueName, false, consumer);
        }

        /// <summary>
        /// 回调接收处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected virtual void ReceivedHandler(object sender, BasicDeliverEventArgs eventArgs)
        {
            try
            {
                bool canAck = false;//应答结果
                int retryCount = 0;//第几次重试
                string messageId = eventArgs.BasicProperties.MessageId;//消息Id
                IDictionary<string, object> headers = eventArgs.BasicProperties.Headers;//头部信息
                if (headers != null && headers.ContainsKey(RabbitMQContext.RetryCountKeyName))
                {
                    retryCount = (int)headers[RabbitMQContext.RetryCountKeyName];
                    Logger.Warn($"队列：{_config.QueueName}，MessageId：{messageId}，第：{retryCount}次重试开始！！！");
                }

                try
                {
                    ConsumerContext context = eventArgs.GetConsumerContext();
                    string pluginPath = "PluginPath";//RabbitMQContext.Config.PluginPath;
                    var instance = SingletonUtil.GetInstance<IConsumer>(pluginPath, _config);
                    ResponseMQResult result = instance.Handler(context);
                    canAck = true;
                    //业务处理成功
                    SuccessHandler(sender, result, eventArgs);
                }
                catch (Exception ex)
                {
                    retryCount++;
                    Logger.Error($"队列：{_config.QueueName}，MessageId：{messageId}，第：{retryCount}次消费发生异常：", ex);
                    canAck = RetryHandler(retryCount, _retryQueueName, eventArgs);
                }
                //处理应答
                AnswerHandler(canAck, eventArgs);
            }
            catch (Exception ex)
            {
                Logger.Error($"队列：{_config.QueueName}，MessageId：{eventArgs.BasicProperties.MessageId}，消费时发生未经处理异常：", ex);
            }
        }

        /// <summary>
        /// 业务处理成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="result">业务处理结果</param>
        /// <param name="eventArgs"></param>
        protected virtual void SuccessHandler(object sender, ResponseMQResult result, BasicDeliverEventArgs eventArgs)
        {

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
                Logger.Error($"MessageId：{messageId}，重试发生异常(RabbitMQ is Closed)：", ex);
            }
        }

        /// <summary>
        /// 重试业务处理方法
        /// </summary> 
        /// <param name="retryCount">当前进行第几次重试</param>
        /// <param name="retryRoutingKey">重试队列路由Key</param>
        /// <param name="eventArgs"></param>
        /// <returns>是否应答，true:应答，false：不应答</returns>
        protected virtual bool RetryHandler(int retryCount, string retryRoutingKey, BasicDeliverEventArgs eventArgs)
        {
            bool canAck = false;//是否应答
            if (_retryTimeRules == null || _retryTimeRules.Count <= 0 || (retryCount > _retryTimeRules.Count))
                return canAck;

            int index = retryCount - 1;
            var properties = eventArgs.BasicProperties;
            properties.Headers[RabbitMQContext.RetryCountKeyName] = retryCount;
            properties.Expiration = _retryTimeRules[index].ToString();
            properties.Headers = properties.Headers ?? new Dictionary<string, object>();
            try
            {
                _channel.BasicPublish(RabbitMQContext.TaskExchangeName, retryRoutingKey, properties, eventArgs.Body);
                canAck = true;//重试一旦发出则标记为应答
            }
            catch (AlreadyClosedException ex)
            {
                Logger.Error($"MessageId：{eventArgs.BasicProperties.MessageId}重试发生异常(RabbitMQ is Closed)：", ex);
            }
            return canAck;
        }
    }
}

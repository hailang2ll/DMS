using DMS.BaseFramework.Common.Extension;
using DMS.RabbitMQ.Context;
using DMS.RabbitMQ.Options;
using DMS.RabbitMQ.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;

namespace DMS.RabbitMQ.Consumers
{
    /// <summary>
    /// 基础消费者
    /// </summary>
    public class ConsumerService
    {

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue">队列名称</param>
        /// <param name="isProperties"></param>
        /// <param name="handler">消费处理</param>
        /// <param name="isDeadLetter"></param>
        public void Subscribe<T>(string queueName, Action<T> handler) where T : class
        {
            //队列声明
            var channel = RabbitMQContext.GetModel(queueName);

            //每次消费的消息数
            channel.BasicQos(0, 1, false);

            RabbitMessageQueueOptions queueOptions = RabbitQueueOptionsUtil.ReaderByQueue(queueName);

            channel.ExchangeDeclare(queueOptions.ExchangeName, ExchangeType.Direct);
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.QueueBind(queueName, queueOptions.ExchangeName, queueOptions.RoutingKeys);


            var consumer = new EventingBasicConsumer(channel);
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
                    Console.WriteLine($"队列：{queueName}，MessageId：{messageId}", ex);
                }
                finally
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            channel.BasicConsume(queueName, false, consumer);
        }
    }
}

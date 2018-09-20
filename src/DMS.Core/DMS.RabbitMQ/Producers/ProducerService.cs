using DMS.BaseFramework.Common.Extension;
using DMS.RabbitMQ.Context;
using DMS.RabbitMQ.Options;
using DMS.RabbitMQ.Utils;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace DMS.RabbitMQ.Producers
{
    /// <summary>
    /// 生产者
    /// </summary>
    public class ProducerService
    {
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">对像实例</param>
        /// <param name="queueName">对列名称</param>
        /// <param name="isCreateQueue">是否创建对列</param>
        public void Publish<T>(T entity, string queueName, bool isCreateQueue = false) where T : class
        {
            byte[] body = entity.SerializeObjectToBytes();

            RabbitMessageQueueOptions queueOptions = RabbitQueueOptionsUtil.ReaderByQueue(queueName);
            var exchangeType = queueOptions.ExchangeType;
            var exchange = queueOptions.ExchangeName;
            var routingKey = queueOptions.RoutingKeys;
            if (isCreateQueue)
            {
                Publish(exchangeType, exchange, routingKey, body, queueName);
            }
            else
            {
                Publish(exchangeType, exchange, routingKey, body);
            }

        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">对像实例</param>
        /// <param name="isCreateQueue">是否创建对列</param>
        public void Publish<T>(T entity, bool isCreateQueue = false) where T : class
        {
            //获取当前队列的特性
            var queueAttrInfo = AttributeUtil.GetRabbitMQAttribute<T>();
            if (queueAttrInfo.IsNullOrEmpty())
                throw new ArgumentException("RabbitMQAttribute未定义");

            var queueName = queueAttrInfo.QueueName;
            var body = entity.SerializeObjectToBytes();

            RabbitMessageQueueOptions queueOptions = RabbitQueueOptionsUtil.ReaderByQueue(queueName);
            var exchangeType = queueOptions.ExchangeType;
            var exchange = queueOptions.ExchangeName;
            var routingKey = queueOptions.RoutingKeys;
            if (isCreateQueue)
            {
                Publish(exchangeType, exchange, routingKey, body, queueName);
            }
            else
            {
                Publish(exchangeType, exchange, routingKey, body);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchangeType">交换机类型</param>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="queueName">对列名称</param>
        /// <param name="routingKey">路由Key</param>
        /// <param name="messageBody">消息字节流</param>
        /// <param name="isProperties">是否持久化</param>
        /// <param name="expirationTime">过期时间，以秒为单位，如“5000”为5秒</param>
        /// <param name="headers"></param>
        private void Publish(string exchangeType, string exchangeName, string routingKey, byte[] messageBody, string queueName = null, bool durable = true, string expirationTime = null, Dictionary<string, object> headers = null)
        {
            var channel = RabbitMQContext.GetModel(routingKey);
            //设置特性
            //var args = new Dictionary<string, object>
            //{
            //    {"x-message-ttl", 5000} //里面的内容自发布起五秒后被删除
            //};

            //var args = new Dictionary<string, object>
            //{
            //    { "x-expires", 5000 } //声明一个queue，queue五秒内而且未被任何形式的消费,则被删除
            //};
            channel.ExchangeDeclare(exchangeName, exchangeType, durable);
            if (!queueName.IsNullOrEmpty())
            {
                channel.QueueDeclare(queueName, true, false, false, null);
                channel.QueueBind(queueName, exchangeName, routingKey);
            }

            var properties = channel.CreateBasicProperties();
            properties.MessageId = Guid.NewGuid().ToString("N");
            properties.DeliveryMode = durable ? (byte)2 : (byte)1;
            properties.Persistent = durable;
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            properties.Headers = headers ?? new Dictionary<string, object> { };
            if (!expirationTime.IsNullOrEmpty())
            {
                properties.Expiration = expirationTime;
            }

            channel.BasicPublish(exchangeName, routingKey, properties, messageBody);
        }
    }
}

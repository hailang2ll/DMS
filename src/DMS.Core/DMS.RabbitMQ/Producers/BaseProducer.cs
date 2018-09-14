using DMS.BaseFramework.Common.Extension;
using DMS.RabbitMQ.Context;
using DMS.RabbitMQ.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.RabbitMQ.Producers
{
    /// <summary>
    /// 生产者
    /// </summary>
    public class BaseProducer
    {
        /// <summary>
        /// 消息连接通道 
        /// </summary>
        protected IModel _channel;
        /// <summary>
        /// 重试队列名称
        /// </summary>
        protected ExchangeConfigOptions _configOptions;
        /// <summary>
        /// 配置文件
        /// </summary>
        protected ExchangeConfigOptions _config;
        public BaseProducer(IModel channel, ExchangeConfigOptions configOptions)
        {
            _channel = channel;
            _configOptions = configOptions;
        }
        /// <summary>
        /// 发布执行处理方法
        /// </summary>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="routingKey"></param>
        /// <param name="messageBody"></param>
        /// <param name="publishTime">发布时间</param>
        /// <param name="durable"></param>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public void Publish(object messageBody)
        {
            _channel.ExchangeDeclare(_configOptions.ExchangeName, _configOptions.ExchangeType);
            int index = _configOptions.Queues.Count;
            foreach (var item in _configOptions.Queues)
            {
                _channel.QueueDeclare(item.QueueName, true, false, false);
                _channel.QueueBind(item.QueueName, _configOptions.ExchangeName, item.RoutingKeys);
            }
            _channel.ConfirmSelect();

            var properties = _channel.CreateBasicProperties();
            properties.MessageId = Guid.NewGuid().ToString("N");
            properties.Persistent = true;  //basicProperties.DeliveryMode = durable ? (byte)2 : (byte)1;//持久化方式
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            properties.Headers = new Dictionary<string, object>
            {
                { "key", "value"}
            };
            //basicProperties.Expiration = ((int)publishTime.Value.Subtract(ctime).TotalMilliseconds).ToString();


            _channel.BasicPublish(_configOptions.ExchangeName, index % 2 == 0 ? "RouteA" : "RouteB", properties, messageBody.SerializeObjectToBytes());
        }
    }
}

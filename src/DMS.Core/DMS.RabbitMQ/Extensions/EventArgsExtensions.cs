using DMS.RabbitMQ.Models;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS.RabbitMQ.Extensions
{
    /// <summary>
    /// 事件数据扩展类
    /// </summary>
    static class EventArgsExtensions
    {
        /// <summary>
        /// 获取消费者处理方法上下文
        /// </summary>
        /// <param name="eventArgs">包含有关从AMQP代理传递的消息的所有信息。</param>
        /// <returns></returns>
        public static ConsumerContext GetConsumerContext(this BasicDeliverEventArgs eventArgs)
        {
            var context = new ConsumerContext
            {
                Body = eventArgs.Body.ToObject(),
                ExchangeName = eventArgs.Exchange,
                RoutingKey = eventArgs.RoutingKey,
                Headers = new Dictionary<string, object>(),
                MessageId = eventArgs.BasicProperties.MessageId,
                ReplyTo = eventArgs.BasicProperties.ReplyTo ?? "",
                CorrelationId = eventArgs.BasicProperties.CorrelationId ?? ""
            };
            var headers = eventArgs.BasicProperties.Headers;//头部信息
            var unixTime = eventArgs.BasicProperties.Timestamp.UnixTime;
            if (unixTime > 0)
            {
                context.TimestampUnix = unixTime;
                context.ConsumerReceiveTime = DateTime.Now;//消费者接收时间
                context.Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(unixTime).LocalDateTime;
                context.TimestampUnixTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTime).UtcDateTime;
            }
            if (headers != null && headers.Any())
            {
                foreach (var header in headers)
                {
                    if (header.Value is byte[] bytes)
                    {
                        context.Headers.Add(header.Key, Encoding.UTF8.GetString(bytes));
                    }
                    else
                    {
                        context.Headers.Add(header.Key, header.Value);
                    }
                }
            }
            return context;
        }
    }
}

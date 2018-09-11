using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.RabbitMQ.Models
{
    /// <summary>
    /// 消费者上下文
    /// </summary>
    public class ConsumerContext 
    {
        /// <summary>
        /// 交换器名称
        /// </summary>
        public string ExchangeName { get; set; }
        /// <summary>
        /// 路由key
        /// </summary>
        public string RoutingKey { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public dynamic Body { get; set; }
        /// <summary>
        /// 消息Id
        /// </summary>
        public string MessageId { get; set; }
        /// <summary>
        /// 消息的生产时间(当前时间)
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public long TimestampUnix { get; set; }
        /// <summary>
        /// 消息的生产时间（Unix时间）
        /// </summary>
        public DateTime TimestampUnixTime { get; set; }
        /// <summary>
        /// 消费者接收时间
        /// </summary>
        public DateTime ConsumerReceiveTime { get; set; }
        /// <summary>
        /// 回复的队列名称
        /// </summary>
        public string ReplyTo { get; set; }
        /// <summary>
        /// 关联Id
        /// </summary>
        public string CorrelationId { get; set; }
        /// <summary>
        /// 头部信息
        /// </summary>
        public IDictionary<string, object> Headers { get; set; }
    }
}

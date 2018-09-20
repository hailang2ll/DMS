using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.RabbitMQ.Attributes
{
    /// <summary>
    /// 自定义的RabbitMq队列信息实体特性
    /// </summary>
    public class RabbitMQAttribute : Attribute
    {
        public RabbitMQAttribute(string queueName)
        {
            QueueName = queueName ?? string.Empty;
        }

        /// <summary>
        /// 交换机名称
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; private set; }

    }
}

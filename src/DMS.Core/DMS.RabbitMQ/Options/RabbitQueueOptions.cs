using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.RabbitMQ.Options
{
    /// <summary>
    /// 对列基本信息
    /// </summary>
    public class RabbitQueueOptions
    {
        /// <summary>
        /// 对列名称
        /// </summary>
        public string QueueName { get; set; }
        /// <summary>
        /// 路由Key
        /// </summary>
        public string RoutingKeys { get; set; }
        /// <summary>
        /// 消息到期时间
        /// </summary>
        public string ExpirationTime { get; set; }


    }
}

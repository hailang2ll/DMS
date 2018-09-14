using DMS.RabbitMQ.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.RabbitMQ.Consumers
{
    /// <summary>
    /// 路由模式
    /// </summary>
    class RoutingConsumer : BaseConsumer
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="config"></param>
        public RoutingConsumer(IModel channel, RabbitmqServiceOptions config)
            : base(channel, config)
        {

        }
    }
}

using DMS.RabbitMQ.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.RabbitMQ.Producers
{
    /// <summary>
    /// 路由模式生产者
    /// </summary>
    class RoutingProducer : BaseProducer
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="channel"></param>
        public RoutingProducer(PublishPatternType patternType)
            : base(patternType)
        {

        }
    }
}

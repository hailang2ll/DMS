
using DMS.RabbitMQ.Attributes;

namespace DMS.RabbitMQ.Enums
{
    /// <summary>
    /// 发布模式类型
    /// </summary>
    public enum PublishPatternType
    {
        /// <summary>
        /// 路由模式
        /// </summary>
        [ExchangeType("direct")]
        Routing = 1,
        /// <summary>
        /// 主题模式
        /// </summary>
        [ExchangeType("topic")]
        Topic = 2,
        /// <summary>
        /// 发布订阅模式
        /// </summary>
        [ExchangeType("fanout")]
        Publish = 3,
        /// <summary>
        /// 订阅模式
        /// </summary>
        /// <summary>
        /// Rpc模式
        /// </summary>
        [ExchangeType("headers")]
        RPC = 4
    }
}
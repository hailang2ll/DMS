
using DMS.RabbitMQ.Attributes;

namespace DMS.RabbitMQ.Enums
{
    /// <summary>
    /// 消费模式类型
    /// </summary>
    public enum ConsumerPatternType
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
        /// 订阅模式
        /// </summary>
        [ExchangeType("fanout")]
        Subscribe = 3,
        /// <summary>
        /// Rpc模式
        /// </summary>
        [ExchangeType("headers")]
        RPC = 4
    }
}
using System;

namespace DMS.RabbitMQ.Attributes
{
    /// <summary>
    /// 交换机类型特性
    /// </summary>
    class ExchangeTypeAttribute : Attribute
    {
        /// <summary>
        /// 交换机类型名称
        /// </summary>
        public string ExchangeType { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="exchangeType">交换机类型</param>
        public ExchangeTypeAttribute(string exchangeType)
        {
            ExchangeType = exchangeType;
        }
    }
}
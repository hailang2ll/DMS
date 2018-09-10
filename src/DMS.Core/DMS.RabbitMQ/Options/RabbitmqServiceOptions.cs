using DMS.RabbitMQ.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.RabbitMQ.Options
{
    /// <summary>
    /// 服务配置
    /// </summary>
    class RabbitmqServiceOptions
    {
        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; set; }
        /// <summary>
        /// 插件的程序集
        /// </summary>
        public string AssemblyName { get; set; }
        /// <summary>
        /// 插件的命名空间
        /// </summary>
        public string NameSpace { get; set; }
        /// <summary>
        /// 消费者的类名称
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 是否持久化（默认：true表示需要持久化）
        /// </summary>
        public bool Durable { get; set; } = true;
        /// <summary>
        /// 是否需要审计(默认：true表示需要审计)
        /// </summary>
        public bool IsAudit { get; set; } = false;
        /// <summary>
        /// 交换机名称
        /// </summary>
        public string ExchangeName { get; set; }
        /// <summary>
        /// 重试的时间规则（单位：秒）
        /// </summary>
        public List<int> RetryTimeRules { get; set; }
        /// <summary>
        /// 模式类型
        /// </summary>
        public ConsumerPatternType PatternType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.RabbitMQ.Options
{
    /// <summary>
    /// 服务配置
    /// </summary>
    public class ExchangeConfigOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string ExchangeName { get; set; }
        /// <summary>
        /// 插件的程序集
        /// </summary>
        public string ExchangeType { get; set; }
        /// <summary>
        /// 插件的命名空间
        /// </summary>
        public bool Persistent { get; set; }

        /// <summary>
        /// 所有对列配置
        /// </summary>
        public List<RabbitQueueOptions> Queues { get; set; }
    }
}

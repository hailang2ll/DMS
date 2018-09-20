using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.RabbitMQ.Options
{
    /// <summary>
    /// 对列信息
    /// </summary>
    public class RabbitMessageQueueOptions : RabbitQueueOptions
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
    }
}

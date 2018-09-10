using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DMS.RabbitMQ.Options
{
    /// <summary>
    /// RabbitMQ配置选项
    /// </summary>
    class RabbitmqOptions
    {
        /// <summary>
        /// 链接字符串
        /// </summary>
        public RabbitConnectionOptions ConnectionString { get; set; }


        /// <summary>
        /// 服务端节点配置
        /// </summary>
        public List<RabbitmqServiceOptions> Services { get; set; }

       
    }
}

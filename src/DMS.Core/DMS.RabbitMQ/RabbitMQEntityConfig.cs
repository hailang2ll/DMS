using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.RabbitMQ
{

    /// <summary>
    /// RabbitMQ
    ///</summary>
    public class RabbitMQConfig
    {

        /// <summary>
        /// MQ服务器
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 虚拟主机
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 心跳数
        /// </summary>
        public ushort RequestedHeartbeat { get; set; }

        /// <summary>
        /// 自动重新连接
        /// </summary>
        public bool AutomaticRecoveryEnabled { get; set; }

        /// <summary>
        ///是否发送
        /// </summary>
        public bool IsSend { get; set; }
    }
}

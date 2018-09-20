using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.RabbitMQ.Options
{
    /// <summary>
    /// 连接字符串
    /// </summary>
    public class RabbitConnectionOptions
    {
        /// <summary>
        /// 主机地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 超时时间（单位：秒）
        /// </summary>
        public ushort TimeOut { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}

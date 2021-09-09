using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Redis.Configurations
{
    public class RedisOption
    {
        /// <summary>
        /// Redis服务器 
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        /// 系统自定义Key前缀
        /// </summary>
        public string RedisPrefixKey { get; set; }
    }
}

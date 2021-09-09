using DMS.Redis.Configurations;
using Microsoft.Extensions.Configuration;
using System;

namespace DMS.Redis
{
    public class AppConfig
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static RedisOption RedisOption
        {
            get
            {
                if (Configuration == null)
                {
                    throw new Exception($"Configuration is null");
                }
                IConfigurationSection configurationSection = Configuration.GetSection("RedisConfig");
                if (configurationSection == null)
                {
                    throw new Exception($"未加载redis.json文件");
                }
                return configurationSection.Get<RedisOption>();
            }

        }
    }
}

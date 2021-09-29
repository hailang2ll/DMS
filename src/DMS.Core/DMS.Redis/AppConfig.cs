using DMS.Redis.Configurations;
using Microsoft.Extensions.Configuration;
using System;

namespace DMS.Redis
{
    public class AppConfig
    {
        public static RedisOption RedisOption
        {
            get
            {
               var Configuration= DMSN.Common.CoreExtensions.AppConfig.Configuration;
                if (Configuration == null)
                {
                    throw new Exception($"Configuration is null,please load AddAppSettingsFile on Startup");
                }
                IConfigurationSection configurationSection = Configuration.GetSection("RedisConfig");
                if (configurationSection == null)
                {
                    throw new Exception($"no load redis.json file");
                }
                return configurationSection.Get<RedisOption>();
            }

        }
    }
}

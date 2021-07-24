using DMS.Redis.Configurations;
using Microsoft.Extensions.Configuration;

namespace DMS.Redis
{
    public class AppConfig
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static RedisOption RedisOption
        {
            get
            {
                return Configuration.GetSection("RedisConfig").Get<RedisOption>();
            }

        }
    }
}

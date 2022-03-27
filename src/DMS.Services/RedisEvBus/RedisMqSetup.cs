using InitQ;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DMS.Services.RedisEvBus
{
    /// <summary>
    /// Redis 消息队列 启动服务
    /// </summary>
    public static class RedisMqSetup
    {
        public static void AddRedisMqSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var option = DMS.Redis.AppConfig.RedisOption;
            if (option.Enabled)
            {
                services.AddInitQ(m =>
                {
                    //时间间隔
                    m.SuspendTime = 2000;
                    //redis服务器地址
                    m.ConnectionString = DMS.Redis.AppConfig.RedisOption.RedisConnectionString;

                    //对应的订阅者类，需要new一个实例对象，当然你也可以传参，比如日志对象
                    m.ListSubscribe = new List<Type>() {
                            typeof(RedisSubscribe)
                    };
                    //显示日志
                    m.ShowLog = false;
                });
            }
        }
    }
}

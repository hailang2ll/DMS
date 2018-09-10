using DMS.Log4net;
using Microsoft.Extensions.Hosting;
using System;

namespace DMS.RabbitMQ.Service.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        /// <summary>
        /// 创建编译
        /// </summary>
        /// <param name="configDir"></param>
        /// <returns></returns>
        static IHost CreateDefaultHost(string configDir) => new HostBuilder()
            .UseLog4net("Config\\log4net.config")
            //.Configure(configDir, "rabbitmq.json")//加载配置文件
            //.UseBusinessHost()//启用业务主机
            //.UseAuditHost()//启用审计队列
            .Build();
    }
}

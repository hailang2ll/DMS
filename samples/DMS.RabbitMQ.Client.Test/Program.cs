using DMS.Log4net;
using DMS.RabbitMQ.Extensions;
using DMS.RabbitMQ.Producers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DMS.RabbitMQ.Client.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateDefaultHost(args).Run();
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="configDir"></param>
        /// <returns></returns>
        static IHost CreateDefaultHost(string[] args) => new HostBuilder()
            .UseLog4net("Config\\log4net.config")
            .UseRabbitMQ("Config\\rabbitmq.json")
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<ProducersService>();
            })
            .Build();
    }

}

using DMS.RabbitMQ.Extensions;
using DMS.RabbitMQ.Models;
using DMS.RabbitMQ.Producers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace DMS.RabbitMQ.Client.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateDefaultHost(args).Run();
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="configDir"></param>
        /// <returns></returns>
        static IHost CreateDefaultHost(string[] args) => new HostBuilder()
            //.UseLog4net("Config\\log4net.config")
            .UseRabbitMQ("Config\\rabbitmq.json")
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<TestHostService>();
            })
            .Build();
    }

}

using DMS.Log4net;
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
            CreateDefaultHost(args);
            ProducerService producerService = new ProducerService();
            var input = Input();
            while (input != "exit")
            {
              
                var model = new MessageBModel
                {
                    CreateDateTime = DateTime.Now,
                    Msg = input
                };


                producerService.Publish(model, "DMS.QueueA");

                input = Input();
            }

        }

        private static string Input()
        {
            Console.WriteLine("请输入信息：");
            var input = Console.ReadLine();
            return input;
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="configDir"></param>
        /// <returns></returns>
        static IHost CreateDefaultHost(string[] args) => new HostBuilder()
            .UseLog4net("Config\\log4net.config")
            .UseRabbitMQ("Config\\rabbitmq.json")
            .Build();
    }

}

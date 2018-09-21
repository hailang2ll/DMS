using DMS.RabbitMQ.Context;
using DMS.RabbitMQ.Models;
using DMS.RabbitMQ.Producers;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DMS.RabbitMQ.Client.Test
{
    /// <summary>
    /// 测试主机
    /// </summary>
    class TestHostService : IHostedService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
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

            return Task.CompletedTask;
        }
        private static string Input()
        {
            Console.WriteLine("请输入信息：");
            var input = Console.ReadLine();
            return input;
        }

        /// <summary>
        /// 停止服务主机
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

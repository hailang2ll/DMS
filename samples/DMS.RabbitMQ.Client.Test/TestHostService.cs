using DMS.RabbitMQ.Context;
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
            var messageBody = new
            {
                Id = 001,
                Name = "张三"
            };
            var headers = new Dictionary<string, object>()
            {
                { "TestKey001","测试头部Key001"},
                { "TestKey002","测试头部Key002"}
            };

            //var config = RabbitMQContext.Config;
            //if (config == null)
            //    throw new TypeInitializationException("RabbitmqConfig", null);

            return Task.CompletedTask;
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

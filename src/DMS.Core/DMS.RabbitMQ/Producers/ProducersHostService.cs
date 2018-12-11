using DMS.RabbitMQ.Context;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DMS.RabbitMQ.Producers
{
    /// <summary>
    /// 消息生产者
    /// </summary>
    public class ProducersHostService : IHostedService
    {
        /// <summary>
        /// 启动服务主机
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

            //var exchangeConfigs = RabbitMQContext.Config.ExchangeConfig;
            //if (exchangeConfigs == null)
            //    throw new TypeInitializationException("ExchangeConfig", null);



            //foreach (var configItem in exchangeConfigs)
            //{
            //    var channel = RabbitMQContext.GetModel(configItem.ExchangeName);
            //    var constructorArgs = new object[] { channel, configItem };

            //    ProducerService baseProducer = new ProducerService(channel, configItem);
            //    baseProducer.Publish("aaaaaaaaa");

            //    Logger.Info($"【消费主机】交换器：{configItem.ExchangeName}配置成功！！！");
            //}

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

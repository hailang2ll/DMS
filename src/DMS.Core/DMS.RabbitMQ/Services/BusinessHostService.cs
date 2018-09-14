using DMS.Log4net;
using DMS.RabbitMQ.Context;
using DMS.RabbitMQ.Options;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DMS.RabbitMQ.Services
{
    class BusinessHostService : IHostedService, IDisposable
    {
        /// <summary>
        /// RabbitMQ建议客户端线程之间不要共用Model，至少要保证共用Model的线程发送消息必须是串行的，但是建议尽量共用Connection
        /// </summary>
        public static readonly ConcurrentDictionary<string, IModel> ChannelDic = new ConcurrentDictionary<string, IModel>();
        /// <summary>
        /// 启动服务
        /// </summary>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = RabbitMQContext.Config;
            if (config == null)
                throw new TypeInitializationException("RabbitmqConfig", null);

            //foreach (ExchangeConfigOptions service in config.Services)
            //{
            //    service.QueueName = service.QueueName.ToLower();
            //    var channel = ChannelDic.GetOrAdd(service.QueueName, RabbitMQContext.Connection.CreateModel());
            //    var constructorArgs = new object[] { channel, service };
            //    ConsumerFactory.GetInstance(service.QueueName, service.PatternType, constructorArgs).Start();
            //    Logger.Info($"【业务主机】队列：{service.QueueName}启动成功！！！");
            //}
            return Task.CompletedTask;
        }

        /// <summary>
        /// 服务停止
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();//释放资源
            Logger.Info($"【业务主机】停止完成！！！");
            return Task.CompletedTask;
        } 

        public void Dispose()
        {
            if (ChannelDic != null && ChannelDic.Any())
            {
                foreach (var channel in ChannelDic.Values)
                {
                    channel?.Close();
                    channel?.Dispose();
                }
            }
            Logger.Info($"【业务主机】资源释放完成！！！");
        }
    }
}

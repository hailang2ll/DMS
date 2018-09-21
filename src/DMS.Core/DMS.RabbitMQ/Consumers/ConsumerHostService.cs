using DMS.BaseFramework.Common.Extension;
using DMS.Log4net;
using DMS.RabbitMQ.Context;
using DMS.RabbitMQ.Models;
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

namespace DMS.RabbitMQ.Consumers
{
    class ConsumerHostService : IHostedService, IDisposable
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
            ConsumerService consumerService = new ConsumerService("DMS.QueueA");

            //被动拉取对列消息，可以用定时任务来处理
            consumerService.Pull<MessageBModel>(msg =>
            {
                //throw new Exception("Always fails!");
                var json = SerializerJson.SerializeObject(msg);
                Console.WriteLine(json);
            });


            //正常消费，不处理异常
            //consumerService.Subscribe<MessageBModel>(msg =>
            //{
            //    throw new Exception("Always fails!");
            //});

            //消费时异常，重新在次消费，可以设置消费的时间延迟消费
            //consumerService.SubscribeRetry<MessageBModel>(msg =>
            //{
            //    throw new Exception("Always fails!");
            //});


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

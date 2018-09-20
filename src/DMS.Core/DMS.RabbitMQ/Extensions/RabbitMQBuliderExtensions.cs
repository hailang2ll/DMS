using DMS.RabbitMQ.Consumers;
using DMS.RabbitMQ.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DMS.RabbitMQ.Extensions
{
    /// <summary>
    /// RabbitMQ扩展
    /// </summary>
    public static class RabbitMQBuliderExtensions
    {
        /// <summary>
        /// 加载rabbitmq配置
        /// 备注：无论是客户端还是服务端必须先加载配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static IHostBuilder UseRabbitMQ(this IHostBuilder builder, string fileName)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var fileDir = Directory.GetCurrentDirectory();
            RabbitMQContext.Configure(fileDir, fileName);
            return builder;
        }
        /// <summary>
        /// 加载rabbitmq配置
        /// 备注：无论是客户端还是服务端必须先加载配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static IWebHostBuilder UseRabbitMQ(this IWebHostBuilder builder, string fileName)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var fileDir = Directory.GetCurrentDirectory();
            RabbitMQContext.Configure(fileDir, fileName);
            return builder;
        }


        #region 启用rabbitmq消费者业务主机
        /// <summary>
        /// 启用rabbitmq消费者业务主机
        /// 备注：如果是消费者类型的项目才需要启用该主机（例如：客户端的生产者则无需启用就可以正常使用）
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="fileDir">配置文件路径</param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static IHostBuilder UseBusinessHost(this IHostBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (RabbitMQContext.Config == null)
                throw new ArgumentNullException(nameof(RabbitMQContext));

            builder.ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<ConsumerHostService>();//业务主机服务
            });
            return builder;
        }
        /// <summary>
        /// 启用rabbitmq消费者业务主机
        /// 备注：如果是消费者类型的项目才需要启用该主机（例如：客户端的生产者则无需启用就可以正常使用）
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="fileDir">配置文件路径</param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static IWebHostBuilder UseBusinessHost(this IWebHostBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (RabbitMQContext.Config == null)
                throw new ArgumentNullException(nameof(RabbitMQContext));

            builder.ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<ConsumerHostService>();//业务主机服务
            });
            return builder;
        }
        #endregion


    }
}

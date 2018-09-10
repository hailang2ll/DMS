using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.RabbitMQ.Extensions
{
    /// <summary>
    /// RabbitMQ扩展
    /// </summary>
    public static class RabbitMQConfigurationExtensions
    {
        /// <summary>
        /// 加载rabbitmq配置
        /// 备注：无论是客户端还是服务端必须先加载配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="fileDir">配置文件路径</param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        //public static IHostBuilder UseRabbitMQTransport(this IHostBuilder builder, string configPath = "")
        //{
        //    if (builder == null)
        //        throw new ArgumentNullException(nameof(builder));

        //    RabbitmqContext.Configure(fileDir, fileName);
        //    return builder;
        //}
        ///// <summary>
        ///// 加载rabbitmq配置
        ///// 备注：无论是客户端还是服务端必须先加载配置
        ///// </summary>
        ///// <param name="builder"></param>
        ///// <param name="fileDir">配置文件路径</param>
        ///// <param name="fileName"></param>
        ///// <returns></returns>
        //public static IWebHostBuilder UseRabbitMQTransport(this IWebHostBuilder builder, string configPath = "")
        //{
        //    if (builder == null)
        //        throw new ArgumentNullException(nameof(builder));

        //    RabbitmqContext.Configure(fileDir, fileName);
        //    return builder;
        //}
    }
}

using DMS.NLogs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace DMS.NLogs
{
    /// <summary>
    /// nlog扩展
    /// </summary>
    public static class NLogExtensions
    {

        /// <summary>
        /// 设置配置文件
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="configPath">配置文件路径，默认调用当前项目执行目录下面的log4net.config作为配置文件</param>
        /// <returns></returns>
        public static void ConfigureNLog(this IConfiguration configuration, string configPath, string basePath = null)
        {
            NLogContext.Configure(configPath, basePath);
        }

        /// <summary>
        /// 使用log4net记录日志
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configPath"></param>
        /// <returns></returns>
        public static IHostBuilder UseNLog(this IHostBuilder builder, string configPath, string basePath = null)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            NLogContext.Configure(configPath, basePath);
            return builder;
        }
        /// <summary>
        ///  使用NLog
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configPath">配置文件路径，默认调用当前项目执行目录下面的log4net.config作为配置文件</param>
        /// <returns></returns>
        public static IWebHostBuilder UseNLog(this IWebHostBuilder builder, string configPath, string basePath = null)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            NLogContext.Configure(configPath, basePath);
            return builder;
        }


    }
}
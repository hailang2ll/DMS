using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace DMS.Log4net
{
    /// <summary>
    /// nlog扩展
    /// </summary>
    public static class Log4Extensions
    {
        /// <summary>
        /// 设置配置文件
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="configPath">配置文件路径，默认调用当前项目执行目录下面的log4net.config作为配置文件</param>
        /// <returns></returns>
        public static void ConfigureLog4net(this IConfiguration configuration, string configPath, string basePath = null)
        {
            Log4Context.Configure(configPath, basePath);
        }

        /// <summary>
        /// 使用log4net记录日志
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configPath"></param>
        /// <returns></returns>
        public static IHostBuilder UseLog4net(this IHostBuilder builder, string configPath, string basePath = null)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            Log4Context.Configure(configPath, basePath);
            return builder;
        }
        /// <summary>
        /// 使用Log4net
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configPath">配置文件路径，默认调用当前项目执行目录下面的log4net.config作为配置文件</param>
        /// <returns></returns>
        public static IWebHostBuilder UseLog4net(this IWebHostBuilder builder, string configPath, string basePath = null)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            Log4Context.Configure(configPath, basePath);
            return builder;
        }
        /// <summary>
        /// 使用Log4net
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configPath">配置文件路径，默认调用当前项目执行目录下面的log4net.config作为配置文件</param>
        /// <returns></returns>
        public static ILoggingBuilder UseLog4net(this ILoggingBuilder builder, string configPath, string basePath = null)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            Log4Context.Configure(configPath, basePath);
            return builder;
        }
    }
}
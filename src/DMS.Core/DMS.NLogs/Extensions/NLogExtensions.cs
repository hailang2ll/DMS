using DMS.NLogs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.IO;

namespace NLogs
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
        public static void ConfigureNLog(this IConfiguration configuration, string configPath = "")
        {
            Configure(configPath);
        }


        /// <summary>
        ///  使用NLog
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configPath">配置文件路径，默认调用当前项目执行目录下面的log4net.config作为配置文件</param>
        /// <returns></returns>
        public static IWebHostBuilder UseNLog(this IWebHostBuilder builder, string configPath = "")
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            Configure(configPath);
            return builder;
        }


        /// <summary>
        /// 设置配置文件
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="configPath">配置文件路径，默认调用当前项目执行目录下面的log4net.config作为配置文件</param>
        /// <returns></returns>
        public static void Configure(string configPath)
        {
            if (string.IsNullOrEmpty(configPath))
            {
                var currentDir = Directory.GetCurrentDirectory();
                configPath = $@"{currentDir}\nlog.config";
            }
            else
            {
                LogFactory factory = NLog.Web.NLogBuilder.ConfigureNLog(configPath);
                NLogContext.Configure(factory);
            }
        }
    }
}
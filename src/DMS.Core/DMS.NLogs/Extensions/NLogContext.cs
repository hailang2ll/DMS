using NLog;
using System;
using System.IO;

namespace DMS.NLogs
{
    /// <summary>
    /// log4net 上下文
    /// </summary>
    class NLogContext
    {
        /// <summary>
        /// 日志工厂
        /// </summary>
        public static LogFactory Factory { get; set; }

        /// <summary>
        /// 配置文件初始化
        /// </summary>
        /// <param name="factory"></param>
        internal static void Configure(LogFactory factory)
        {
            Factory = factory;
        }


        /// <summary>
        /// 设置配置文件
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="configPath">配置文件路径，默认调用当前项目执行目录下面的log4net.config作为配置文件</param>
        /// <returns></returns>
        public static void Configure(string configPath, string basePath = null)
        {
            if (!string.IsNullOrEmpty(basePath))
            {
                configPath = Path.Combine(basePath, configPath);
            }
            else
            {
                configPath = Path.Combine(AppContext.BaseDirectory, configPath);
            }
            FileInfo file = new FileInfo(configPath);
            if (file.Exists)
            {
                LogFactory factory = NLog.Web.NLogBuilder.ConfigureNLog(configPath);
                Configure(factory);
                if (factory.IsLoggingEnabled())
                {
                    Logger.Info($"初始化{configPath}完成。");
                }
            }
            else
            {
                throw new Exception($"未找到{file.FullName}文件");
            }

        }
    }
}
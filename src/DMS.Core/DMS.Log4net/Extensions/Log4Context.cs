using log4net;
using log4net.Repository;
using System;
using System.IO;

namespace DMS.Log4net
{
    /// <summary>
    /// 
    /// </summary>
    class Log4Context
    {
        /// <summary>
        /// 系统日志记录器
        /// </summary>
        protected internal static ILog SysLog { get; private set; }

        /// <summary>
        /// 程序异常日志记录器
        /// </summary>
        protected internal static ILog ExceptionLog { get; private set; }

        /// <summary>
        /// 基础服务日志记录器
        /// </summary>
        protected internal static ILog WaLiuBasicServiceLog { get; private set; }

        /// <summary>
        /// API监控日志
        /// </summary>
        protected internal static ILog ApiMonitorLog { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configPath"></param>
        public static void Configure(string configPath)
        {
            FileInfo file = new FileInfo(configPath);
            if (file.Exists)
            {
                ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
                log4net.Config.XmlConfigurator.ConfigureAndWatch(repository, file);

                SysLog = LogManager.GetLogger(repository.Name, "SystemLogger");
                ExceptionLog = LogManager.GetLogger(repository.Name, "ExceptionLogger");

                WaLiuBasicServiceLog = LogManager.GetLogger(repository.Name, "WaLiuBasicServiceLogger");
                ApiMonitorLog = LogManager.GetLogger(repository.Name, "ApiMonitorLogger");
                SysLog.Info($"初始化{configPath}完成。");
            }
            else
            {
                throw new Exception($"未找到{file.FullName}文件");
            }
        }
    }
}
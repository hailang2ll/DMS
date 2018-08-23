using NLog;

namespace DMS.NLog
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
    }
}
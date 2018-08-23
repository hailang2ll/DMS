using NLog;
using System;

namespace DMS.NLogs
{
    /// <summary>
    /// 
    /// </summary>
    public class NLogger
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        public static readonly Logger _log = NLogContext.Factory.GetCurrentClassLogger();

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            _log.Debug(message);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Debug(string message, Exception ex)
        {
            _log.Debug(ex, message);
        }

        /// <summary>
        /// 消息日志
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            _log.Info(message);
        }

        /// <summary>
        /// 消息日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Info(string message, Exception ex)
        {
            _log.Info(ex, message);
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            _log.Warn(message);
        }

        /// <summary>
        /// 消息日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Warn(string message, Exception ex)
        {
            _log.Warn(ex, message);
        }

        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            _log.Error(message);
        }

        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Error(string message, Exception ex)
        {
            _log.Error(ex, message);
        }
    }
}
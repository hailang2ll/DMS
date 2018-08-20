using Exceptionless.Logging;
using DMS.Exceptionless.Common;
using DMS.Exceptionless.Param;
using System.Collections.Generic;

namespace DMS.Exceptionless
{
    /// <summary>
    /// 消息日志
    /// </summary>
    public class LessLog : BaseLessLog
    {
        #region Info
        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        public static void Info(string message, params string[] tags)
        {
            Submit(message, LogLevel.Info, tags);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="tags">标签</param>
        public static void Info(string message, ExcUserParam user, params string[] tags)
        {
            Submit(message, LogLevel.Info, user, tags);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Info(string message, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Info, data, tags);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Info(string message, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Info, datas, tags);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Info(string message, ExcUserParam user, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Info, user, data, tags);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Info(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Info, user, datas, tags);
        }
        #endregion

        #region Error
        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        public static void Error(string message, params string[] tags)
        {
            Submit(message, LogLevel.Error, tags);
        }

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="tags">标签</param>
        public static void Error(string message, ExcUserParam user, params string[] tags)
        {
            Submit(message, LogLevel.Error, user, tags);
        }

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Error(string message, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Error, data, tags);
        }

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Error(string message, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Error, datas, tags);
        }

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Error(string message, ExcUserParam user, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Error, user, data, tags);
        }

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Error(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Error, user, datas, tags);
        }
        #endregion

        #region Trace
        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        public static void Trace(string message, params string[] tags)
        {
            Submit(message, LogLevel.Trace, tags);
        }

        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="tags">标签</param>
        public static void Trace(string message, ExcUserParam user, params string[] tags)
        {
            Submit(message, LogLevel.Trace, user, tags);
        }

        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Trace(string message, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Trace, data, tags);
        }

        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Trace(string message, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Trace, datas, tags);
        }

        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Trace(string message, ExcUserParam user, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Trace, user, data, tags);
        }

        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Trace(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Trace, user, datas, tags);
        }
        #endregion

        #region Debug
        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        public static void Debug(string message, params string[] tags)
        {
            Submit(message, LogLevel.Debug, tags);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="tags">标签</param>
        public static void Debug(string message, ExcUserParam user, params string[] tags)
        {
            Submit(message, LogLevel.Debug, user, tags);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Debug(string message, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Debug, data, tags);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Debug(string message, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Debug, datas, tags);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Debug(string message, ExcUserParam user, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Debug, user, data, tags);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Debug(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Debug, user, datas, tags);
        }
        #endregion

        #region Warn
        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        public static void Warn(string message, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, tags);
        }

        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="tags">标签</param>
        public static void Warn(string message, ExcUserParam user, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, user, tags);
        }

        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Warn(string message, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, data, tags);
        }

        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Warn(string message, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, datas, tags);
        }

        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Warn(string message, ExcUserParam user, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, user, data, tags);
        }

        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Warn(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Warn, user, datas, tags);
        }
        #endregion

        #region Fatal
        /// <summary>
        /// 致命的消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        public static void Fatal(string message, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, tags);
        }

        /// <summary>
        /// 致命的消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="tags">标签</param>
        public static void Fatal(string message, ExcUserParam user, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, user, tags);
        }

        /// <summary>
        /// 致命的消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Fatal(string message, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, data, tags);
        }

        /// <summary>
        /// 致命的消息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Fatal(string message, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, datas, tags);
        }

        /// <summary>
        /// 致命的消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Fatal(string message, ExcUserParam user, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, user, data, tags);
        }

        /// <summary>
        /// 致命的消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Fatal(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, user, datas, tags);
        }
        #endregion
    }
}
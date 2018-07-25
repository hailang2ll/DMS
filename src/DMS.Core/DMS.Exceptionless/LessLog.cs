using Exceptionless;
using Exceptionless.Configuration;
using Exceptionless.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using DMS.BaseFramework.Common.Configuration;

namespace DMS.Exceptionless
{
    public class LessLog
    {
        static LessLog()
        {
            ExceptionlessClient.Default.Configuration.ApiKey = AppSettings.GetValue("Exceptionless:ApiKey");
            ExceptionlessClient.Default.Configuration.ServerUrl = AppSettings.GetValue("Exceptionless:ServerUrl");
        }

        #region Info
        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="message">消息</param>
        public static void Info(string message)
        {
            //string methodName0 = new System.Diagnostics.StackTrace(true).GetFrame(1).GetMethod().Name;//事件源,OnClick,但是不显示writeerror这个方法名。。
            string methodName = new System.Diagnostics.StackTrace(true).GetFrame(1).GetMethod().DeclaringType.ToString();
            Info(methodName, message);
        }
        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="source">来源：typeof(LessLog).FullName</param>
        /// <param name="message">消息</param>
        public static void Info(string source, string message)
        {
            ExceptionlessClient.Default.SubmitLog(source, message, LogLevel.Info);
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="type">类型来源</param>
        /// <param name="message">消息</param>
        public static void Info(Type type, string message)
        {
            ExceptionlessClient.Default.SubmitLog(type.FullName, message, LogLevel.Info);
        }
        #endregion

        #region Error
        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="message">消息</param>
        public static void Error(string message)
        {
            //string methodName0 = new System.Diagnostics.StackTrace(true).GetFrame(1).GetMethod().Name;//事件源,OnClick,但是不显示writeerror这个方法名。。
            string methodName = new System.Diagnostics.StackTrace(true).GetFrame(1).GetMethod().DeclaringType.ToString();
            Error(methodName, message);
        }
        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="source">来源：typeof(LessLog).FullName</param>
        /// <param name="message">消息</param>
        public static void Error(string source, string message)
        {
            ExceptionlessClient.Default.SubmitLog(source, message, LogLevel.Error);
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="type">类型来源</param>
        /// <param name="message">消息</param>
        public static void Error(Type type, string message)
        {
            ExceptionlessClient.Default.SubmitLog(type.FullName, message, LogLevel.Error);
        }
        #endregion

        #region Fatal
        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="message">消息</param>
        public static void Fatal(string message)
        {
            //string methodName0 = new System.Diagnostics.StackTrace(true).GetFrame(1).GetMethod().Name;//事件源,OnClick,但是不显示writeerror这个方法名。。
            string methodName = new System.Diagnostics.StackTrace(true).GetFrame(1).GetMethod().DeclaringType.ToString();
            Fatal(methodName, message);
        }
        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="source">来源：typeof(LessLog).FullName</param>
        /// <param name="message">消息</param>
        public static void Fatal(string source, string message)
        {
            ExceptionlessClient.Default.SubmitLog(source, message, LogLevel.Fatal);
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="type">类型来源</param>
        /// <param name="message">消息</param>
        public static void Fatal(Type type, string message)
        {
            ExceptionlessClient.Default.SubmitLog(type.FullName, message, LogLevel.Fatal);
        }
        #endregion

        #region Exception
        /// <summary>
        /// 异常信息
        /// </summary>
        /// <param name="exception"></param>
        public static void Exception(Exception exception)
        {
            exception.ToExceptionless().Submit();
        }

        /// <summary>
        /// 异常信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Exception(string message, Exception exception)
        {
            exception.ToExceptionless().SetMessage(message + "======>" + exception.Message).Submit();
        }
        #endregion
    }
}

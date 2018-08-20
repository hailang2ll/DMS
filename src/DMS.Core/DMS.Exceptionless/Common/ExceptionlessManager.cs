using DMS.BaseFramework.Common.Configuration;
using Exceptionless;
using Exceptionless.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DMS.Exceptionless.Common
{
    /// <summary>
    /// 异常管理类
    /// </summary>
    public class ExceptionlessManager
    {
        /// <summary>
        /// 提交事件,提供给外部订阅使用
        /// </summary>
        public static event EventHandler<EventSubmittingEventArgs> SubmittingEvent;

        /// <summary>
        /// 异常日志客户端
        /// </summary>
        protected internal static ExceptionlessManager Instance = new ExceptionlessManager();

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ExceptionlessManager()
        {
            ExceptionlessClient.Default.Configuration.ApiKey = AppSettings.GetValue("Exceptionless:ApiKey");
            ExceptionlessClient.Default.Configuration.ServerUrl = AppSettings.GetValue("Exceptionless:ServerUrl");
            ExceptionlessClient.Default.SubmittingEvent += Default_SubmittingEvent; ;
        }

        /// <summary>
        /// 转换为异常信息
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected internal EventBuilder ToExceptionless(Exception exception)
        {
            return exception.ToExceptionless();
        }

        /// <summary>
        /// 转换为消息日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        protected internal EventBuilder ToMessageLess(string message, LogLevel level)
        {
            return ExceptionlessClient.Default.CreateLog(message, level);
        }

        /// <summary>
        /// 转换为特性日志
        /// </summary>
        /// <param name="feature">特性信息</param>
        /// <returns></returns>
        protected internal EventBuilder ToFeatureUsageLess(string feature)
        {
            return ExceptionlessClient.Default.CreateFeatureUsage(feature);
        }

        /// <summary>
        /// 转换为失效链接
        /// </summary>
        /// <param name="resource">链接地址</param>
        /// <returns></returns>
        protected internal EventBuilder ToNotFound(string resource)
        {
            return ExceptionlessClient.Default.CreateNotFound(resource);
        }

        /// <summary>
        /// 默认提交异常处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Default_SubmittingEvent(object sender, EventSubmittingEventArgs e)
        {
            if (SubmittingEvent != null)
            {
                //如果客户端自定义提交异常，则走自定义，否则走默认的异常处理事件
                SubmittingEvent(sender, e);
            }
            else
            {
                // 只处理未处理的异常
                if (!e.IsUnhandledError)
                    return;

                //忽略没有错误体的错误
                var error = e.Event.GetError();
                if (error == null)
                    return;

                // 忽略404错误
                if (e.Event.IsNotFound())
                {
                    e.Cancel = true;
                    return;
                }

                //忽略401(Unauthorized)和请求验证的错误.
                if (error.Code == "401" || error.Type == "System.Web.HttpRequestValidationException")
                {
                    e.Cancel = true;
                    return;
                }

                //忽略任何未被代码抛出的异常
                var handledNamespaces = new List<string> { "Exceptionless" };
                var handledNamespaceList = error.StackTrace.Select(s => s.DeclaringNamespace).Distinct();
                if (!handledNamespaceList.Any(ns => handledNamespaces.Any(ns.Contains)))
                {
                    e.Cancel = true;
                    return;
                }

                e.Event.Tags.Add("未捕获异常");//添加系统异常标签
                e.Event.MarkAsCritical();//标记为关键异常
            }
        }
    }
}
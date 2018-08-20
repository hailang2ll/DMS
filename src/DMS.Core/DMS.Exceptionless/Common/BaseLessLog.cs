using DMS.Exceptionless.Param;
using Exceptionless;
using Exceptionless.Logging;
using System;
using System.Collections.Generic;

namespace DMS.Exceptionless.Common
{
    /// <summary>
    /// 日志基础处理类
    /// </summary>
    public class BaseLessLog
    {
        /// <summary>
        /// 提交消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="level"></param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        protected static void Submit(string message, LogLevel level, params string[] tags)
        {
            Submit(message, level, data: null, tags: tags);
        }

        /// <summary>
        /// 提交消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="level">日志等级</param>
        /// <param name="user">用户</param>
        /// <param name="tags">标签</param>
        protected static void Submit(string message, LogLevel level, ExcUserParam user, params string[] tags)
        {
            Submit(message, level, user, data: null, tags: tags);
        }

        /// <summary>
        /// 提交消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="level">日志等级</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        protected static void Submit(string message, LogLevel level, ExcDataParam data, params string[] tags)
        {
            Submit(message, level, null, data, tags);
        }

        /// <summary>
        /// 提交消息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="level">日志等级</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        protected static void Submit(string message, LogLevel level, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, level, null, datas, tags);
        }

        /// <summary>
        /// 提交消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="level">日志等级</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        protected static void Submit(string message, LogLevel level, ExcUserParam user, ExcDataParam data, params string[] tags)
        {
            var datas = new List<ExcDataParam>() { data };
            Submit(message, level, user, datas, tags);
        }

        /// <summary>
        /// 提交消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="level">日志等级</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        protected static void Submit(string message, LogLevel level, ExcUserParam user, List<ExcDataParam> datas, params string[] tags)
        {
            EventBuilder builder = ExceptionlessManager.Instance.ToMessageLess(message, level)
               .AddTags(tags)
               .SetUserIdentity(user?.Id, user?.Name)
               .SetUserDescription(user?.Email, user?.Description)
               .SetReferenceId(Guid.NewGuid().ToString("N"));

            if (datas?.Count > 0)
            {
                foreach (var data in datas)
                {
                    builder.AddObject(data?.Data, data?.Name);
                }
            }
            builder.Submit();
        }
    }
}
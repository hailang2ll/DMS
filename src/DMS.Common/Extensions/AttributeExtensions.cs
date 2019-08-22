using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DMS.Common.Extensions
{
    #region demo
    [RabbitMq("Test.QueueName", ExchangeName = "Test.ExchangeName", IsProperties = false)]
    public class MessageModel
    {
        public string Msg { get; set; }

        public DateTime CreateDateTime { get; set; }
    }

    /// <summary>
    /// Demo自定义的RabbitMq队列信息实体特性
    /// </summary>
    public class RabbitMqAttribute : Attribute
    {
        public RabbitMqAttribute(string queueName)
        {
            QueueName = queueName ?? string.Empty;
        }

        /// <summary>
        /// 交换机名称
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; private set; }

        /// <summary>
        /// 是否持久化
        /// </summary>
        public bool IsProperties { get; set; }
    }
    #endregion


    public static class AttributeExtensions
    {
        public static T GetCustomAttribute<T>(this object obj) where T : class
        {
            return obj.GetType().GetCustomAttribute<T>();
        }

        public static T GetCustomAttribute<T>(this Type type) where T : class
        {
            Attribute customAttribute = type.GetCustomAttribute(typeof(T));
            if (!customAttribute.IsNullOrEmpty())
            {
                return (customAttribute as T);
            }
            return default(T);
        }
    }
}

using DMS.RabbitMQ.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DMS.RabbitMQ.Extensions
{
    /// <summary>
    /// 枚举的扩展类
    /// </summary>
    static class EnumExtensions
    {
        /// <summary>
        /// 获取任务路由Key
        /// </summary>
        /// <param name="routingKey"></param>
        public static string GetTaskRoutingKey(this string routingKey)
        {
            return $"{routingKey ?? ""}.task";
        }

        /// <summary>
        /// 获取交换机名称
        /// </summary>
        /// <param name="patternType"></param>
        /// <param name="exchangeName">交换机名称</param>
        /// <returns></returns>
        public static string GetExchangeName(this Enum patternType, string exchangeName)
        {
            return string.IsNullOrEmpty(exchangeName) ?
                $"stack.rabbitmq.{patternType.ToString().ToLower()}"
                : exchangeName.ToLower();
        }
        /// <summary>
        /// 获取交换机类型
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static string GetExchangeType(this Enum enumType)
        {
            Type temType = enumType.GetType();
            MemberInfo[] memberInfos = temType.GetMember($"{enumType}");
            if (memberInfos == null || !memberInfos.Any())
                return "";

            MemberInfo memberInfo = memberInfos.FirstOrDefault();
            object[] attributes = memberInfo.GetCustomAttributes(typeof(ExchangeTypeAttribute), false);
            if (attributes == null || attributes.Length <= 0)
                return "";

            var firstAttribute = (ExchangeTypeAttribute)attributes.FirstOrDefault();
            return firstAttribute.ExchangeType;
        }
    }
}

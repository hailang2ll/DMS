using DMS.RabbitMQ.Consumers;
using DMS.RabbitMQ.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DMS.RabbitMQ.Extensions
{
    /// <summary>
    /// RabbitMQ扩展
    /// </summary>
    public static class RabbitMQExtensions
    {
        /// <summary>
        /// 获取当前重试后缀
        /// </summary>
        /// <param name="currKey"></param>
        /// <returns></returns>
        public static string GetRetrySuffixName(this string currKey)
        {
            return $"{currKey ?? ""}_Retry";
        }
    }
}

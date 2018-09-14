using DMS.RabbitMQ.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.RabbitMQ.Consumers
{
    /// <summary>
    /// 消费者工厂
    /// </summary>
    class ConsumerFactory
    {
        /// <summary>
        /// 确保线程同步的对象锁
        /// </summary>
        private static readonly object locker = new object();
        /// <summary>
        /// 缓存字典
        /// </summary>
        private static Dictionary<string, BaseConsumer> InstanceCacheDic = new Dictionary<string, BaseConsumer>();
        /// <summary>
        /// 根据配置类型获取对象实例
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="patternType">消费者类型</param>
        /// <param name="constructorArgs">可变的构造函数列表</param>
        /// <returns></returns>
        public static BaseConsumer GetInstance(string queueName, ConsumerPatternType patternType, params object[] constructorArgs)
        {
            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentException("queueName");

            if (!InstanceCacheDic.ContainsKey(queueName))
            {
                lock (locker)
                {
                    string assemblyName = "DMS.RabbitMQ.Consumers";
                    string className = $"{assemblyName}.{patternType.ToString()}Consumer";
                    BaseConsumer instance = (BaseConsumer)Activator.CreateInstance(Type.GetType(className), constructorArgs);
                    InstanceCacheDic.Add(queueName, instance);
                }
            }
            return InstanceCacheDic[queueName];
        }
    }
}

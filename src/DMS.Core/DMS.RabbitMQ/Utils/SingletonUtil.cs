using DMS.RabbitMQ.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.RabbitMQ.Utils
{
    /// <summary>
    /// 单利处理类
    /// </summary>
    sealed class SingletonUtil
    {
        private static readonly object _lock = new object();
        private static Dictionary<string, object> instanceDic = new Dictionary<string, object>();
        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <param name="pluginPath"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static T GetInstance<T>(string pluginPath, RabbitmqServiceOptions config) where T : class
        {
            return GetInstance<T>(pluginPath, config.AssemblyName, config.NameSpace, config.ClassName);
        }
        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <param name="assemblyName">类型所在程序集名称</param>
        /// <param name="nameSpace">类型所在命名空间</param>
        /// <param name="className">类型名</param>
        /// <returns></returns>
        public static T GetInstance<T>(string filePath, string assemblyName, string nameSpace, string className) where T : class
        {
            var fullName = ReflectionUtil.GetFullName(nameSpace, className);
            if (!instanceDic.ContainsKey(fullName))
            {
                lock (_lock)
                {
                    if (!instanceDic.ContainsKey(fullName))
                    {
                        var instance = ReflectionUtil.CreateInstance(filePath, assemblyName, nameSpace, className);
                        instanceDic.Add(fullName, instance);
                    }
                }
            }
            return (T)instanceDic[fullName];
        }
    }
}

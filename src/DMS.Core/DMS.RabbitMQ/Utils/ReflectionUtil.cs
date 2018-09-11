using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DMS.RabbitMQ.Utils
{
    /// <summary>
    /// 反射业务处理类
    /// </summary>
    public sealed class ReflectionUtil
    {
        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <param name="filePath">程序集文件存放路径</param>
        /// <param name="assemblyName">程序集</param>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public static object CreateInstance(string filePath, string assemblyName, string nameSpace, string className)
        {
            Assembly assembly = Assembly.LoadFrom($@"{filePath}\{assemblyName}.dll");
            var fullName = GetFullName(nameSpace, className);
            Type type = assembly.GetType(fullName);
            return Activator.CreateInstance(type);
        }
        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullName">命名空间.类型名</param>
        /// <param name="assemblyName">程序集</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string fullName, string assemblyName)
        {
            string path = fullName + "," + assemblyName;//命名空间.类型名,程序集
            Type o = Type.GetType(path);//加载类型
            return (T)Activator.CreateInstance(o, true);//根据类型创建实例
        }
        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T">要创建对象的类型</typeparam>
        /// <param name="assemblyName">类型所在程序集名称</param>
        /// <param name="nameSpace">类型所在命名空间</param>
        /// <param name="className">类型名</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string assemblyName, string nameSpace, string className)
        {
            string fullName = GetFullName(nameSpace, className);//命名空间.类型名
            return CreateInstance<T>(fullName, assemblyName);
        }
        /// <summary>
        /// 获取文件全称（命名空间.类型名）
        /// </summary>
        /// <param name="nameSpace">类型所在命名空间</param>
        /// <param name="className">类型名</param>
        /// <returns></returns>
        public static string GetFullName(string nameSpace, string className)
        {
            return nameSpace + "." + className;//命名空间.类型名
        }
    }
}

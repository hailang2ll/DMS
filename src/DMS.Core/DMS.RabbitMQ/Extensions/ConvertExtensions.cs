using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.RabbitMQ.Extensions
{
    /// <summary>
    /// 数据转换扩展类
    /// </summary>
    static class ConvertExtensions
    {
        /// <summary> 
        /// 将一个object对象序列化，返回一个byte[]         
        /// </summary> 
        /// <param name="obj">能序列化的对象</param>         
        /// <returns></returns> 
        public static byte[] ToBytes(this object obj)
        {
            var messageStrings = JsonConvert.SerializeObject(obj);
            return Encoding.UTF8.GetBytes(messageStrings);
        }

        /// <summary> 
        /// 将一个序列化后的byte[]数组还原         
        /// </summary>
        /// <param name="bytes"></param>         
        /// <returns></returns> 
        public static object ToObject(this byte[] bytes)
        {
            var messageStrings = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<object>(messageStrings);
        }

        /// <summary> 
        /// 将一个序列化后的byte[]数组还原         
        /// </summary>
        /// <param name="bytes"></param>         
        /// <returns></returns> 
        public static T ToObject<T>(this byte[] bytes)
        {
            var messageStrings = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(messageStrings);
        }
    }
}

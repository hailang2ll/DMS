using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Text;

namespace DMS.Common.Extensions
{
    public static class SerializerExtensions
    {
        /// <summary>
        /// 将实体序列化成字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObject(this object obj)
        {
            JsonSerializerSettings settings = DefaultJsonSettings();
            return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None, settings);
        }
        /// <summary>
        /// 将字符串反序列化成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(this string value)
        {
            JsonSerializerSettings settings = DefaultJsonSettings();
            return JsonConvert.DeserializeObject<T>(value, settings);
        }


        /// <summary> 
        /// 将一个object对象序列化，返回一个byte[]         
        /// </summary> 
        /// <param name="obj">能序列化的对象</param>         
        /// <returns></returns> 
        public static byte[] SerializeBytes(this object obj)
        {
            JsonSerializerSettings settings = DefaultJsonSettings();
            var messageStrings = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None, settings);
            return Encoding.UTF8.GetBytes(messageStrings);
        }
        /// <summary>
        /// 反序列化成实体
        /// </summary>
        /// <typeparam name="T">具体实体</typeparam>
        /// <param name="data">byte字节流</param>
        /// <returns>返回一个实体</returns>
        public static T DeserializeBytes<T>(this byte[] data)
        {
            string json = Encoding.UTF8.GetString(data);
            T entity = DeserializeObject<T>(json);
            return entity;
        }


        /// <summary>
        /// 序列化成字节流
        /// </summary>
        /// <typeparam name="T">泛型实体</typeparam>
        /// <param name="entity">具体实体</param>
        /// <returns>byte字节流</returns>
        //public static byte[] SerializeObject<T>(this T entity)
        //{
        //    XmlSerializer xs = new XmlSerializer(typeof(T));
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        xs.Serialize(ms, entity);
        //        byte[] bytes = ms.ToArray();
        //        return bytes;
        //    }
        //}




        #region private
        /// <summary>
        /// 序列化转化方式
        /// </summary>
        /// <returns></returns>
        private static JsonSerializerSettings DefaultJsonSettings()
        {

            JsonSerializerSettings json = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
                //DefaultValueHandling = DefaultValueHandling.Ignore
            };
            json.Converters.Clear();
            json.Converters.Add(new IsoDateTimeConverter()
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            });
            //json.Converters.Add(new CustomStringConverter());
            return json;
        }
        #endregion
    }

    /// <summary>
    /// 处理Long类型问题
    /// </summary>
    public class CustomStringConverter : CustomCreationConverter<long>
    {
        /// <summary>
        /// 
        /// </summary>
        public CustomStringConverter()
        {
        }

        /// <summary>
        /// 重载是否可写
        /// </summary>
        public override bool CanWrite { get { return true; } }

        public override long Create(Type objectType)
        {
            return Int64.MaxValue;
        }

        /// <summary>
        /// 重载序列化方法
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(value.ToString());
            }

        }
    }
}

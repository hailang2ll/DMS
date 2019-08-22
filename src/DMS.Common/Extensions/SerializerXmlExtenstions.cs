using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DMS.Common.Extensions
{
    public static class SerializerXmlExtenstions
    {
        #region 配置文件

        /// <summary>
        /// 从配置文件中反序列化对象
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        public static object LoadSettings(string filePath, Type type)
        {
            FileInfo file = new FileInfo(filePath);
            if (!file.Exists) return null;

            XmlSerializer serializer = new XmlSerializer(type);
            FileStream stream = file.OpenRead();
            object result = serializer.Deserialize(stream);
            stream.Close();

            return result;
        }

        /// <summary>
        /// 对象序列化到配置文件中
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="value">要序列化的对象</param>
        public static void SaveSettings(string filePath, object value)
        {
            XmlSerializer mySerializer = new XmlSerializer(value.GetType());
            StreamWriter myWriter = new StreamWriter(filePath, false);

            mySerializer.Serialize(myWriter, value);
            myWriter.Close();
        }

        #endregion

        #region 序列化/反序列化

        /// <summary>
        /// 对象序列化成 XML
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// XML 反序列化成对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="xml">XML</param>
        /// <returns></returns>
        public static T Deserializer<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                XmlReader xmlReader = XmlReader.Create(stream);
                return (T)serializer.Deserialize(xmlReader);
            }
        }

        /// <summary>
        /// XML 反序列化成对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="xml">XML</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static T Deserializer<T>(string xml, Encoding encoding)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (Stream stream = new MemoryStream(encoding.GetBytes(xml)))
            {
                return (T)serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// XML 反序列化成对象
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">xml数据</param>
        /// <returns></returns>
        public static object Deserializer(Type type, string xml)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                XmlReader xmlReader = XmlReader.Create(stream);
                return serializer.Deserialize(xmlReader);
            }
        }

        /// <summary>
        /// 对象序列化成 XML
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string SerializerNoXmlLabel(object obj)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            //settings.Encoding = Encoding.UTF8;
            settings.OmitXmlDeclaration = true;
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, obj, ns);
                    return Encoding.UTF8.GetString(stream.ToArray());
                }
            }
        }

        /// <summary>
        /// 对象序列化成 XML
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string SerializerNoXmlLabel(object obj, Encoding encoding)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = encoding;
            settings.OmitXmlDeclaration = true;
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, obj, ns);
                    return encoding.GetString(stream.ToArray());
                }
            }
        }

        /// <summary>
        /// 对象序列化成 XML
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string Serializer(object obj, Encoding encoding)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = encoding;
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, obj);
                    return encoding.GetString(stream.ToArray());
                }
            }
        }

        #endregion
    }
}

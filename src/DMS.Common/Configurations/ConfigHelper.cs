using DMS.Common.Extensions;
using System;
using System.Xml;

namespace TYSystem.BaseFramework.Common.Helper
{
    /// <summary>
    /// ConfigPath帮助类
    /// 主要是为了实现在File下面的配置路径并生成XML实体
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 获取配置项的值
        /// </summary>
        /// <param name="key">健值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetValue(string key, string defaultValue)
        {
            var value = System.Configuration.ConfigurationManager.AppSettings[key];
            if (value.IsNullOrEmpty())
                value = defaultValue;
            return value;
        }



        #region 读取Config目录下的文件

        public static XmlDocument GetXmlDoc(string configPath)
        {
            try
            {
                //string configPath = GetConfigPath + configName + ".xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configPath);
                return xmlDoc;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("初始化配置文件{0}出错：" + ex.Message + ",StackTrace=" + ex.StackTrace, configPath));
            }

        }

        public static string SelectSingleNode(string configName, string singleNode)
        {
            XmlDocument xmlDoc = GetXmlDoc(configName);
            if (xmlDoc == null)
            {
                return "";
            }
            string value = "";
            try
            {
                value = xmlDoc.SelectSingleNode("//" + configName + "ConfigInfo/" + singleNode).InnerText;
            }
            catch
            {
            }
            return value;
        }
        public static XmlNodeList SelectNodeList(string configName, string singleNode)
        {
            XmlDocument xmlDoc = GetXmlDoc(configName);
            if (xmlDoc != null)
            {
                XmlNodeList xmlNodeList = xmlDoc.SelectNodes("//" + configName + "ConfigInfo/" + singleNode);
                return xmlNodeList;
            }
            return null;
        }
        public static XmlNode SelectSingleAttrNode(string configName, string singleNode, string attrWhere, string attrWhereValue)
        {
            XmlDocument xmlDoc = GetXmlDoc(configName);
            if (xmlDoc != null)
            {
                XmlNodeList xmlNodeList = xmlDoc.SelectNodes("//" + configName + "ConfigInfo/" + singleNode + "[@" + attrWhere + "='" + attrWhereValue + "']");
                if (xmlNodeList.Count < 1)
                {
                    throw new Exception("文件处理配置文件不存在" + attrWhere);
                }
                return xmlNodeList[0];
            }
            return null;
        }
        public static string SelectSingleAttrNode(string configName, string singleNode, string attrWhere, string attrWhereValue, string attrResult)
        {
            string directoryName = string.Empty;
            XmlDocument xmlDoc = GetXmlDoc(configName);
            if (xmlDoc != null)
            {
                XmlNodeList xmlNodeList = xmlDoc.SelectNodes("//" + configName + "ConfigInfo/" + singleNode + "[@" + attrWhere + "='" + attrWhereValue + "']");
                if (xmlNodeList.Count < 1)
                {
                    throw new Exception("文件处理配置文件不存在" + attrWhere);
                }

                directoryName = xmlNodeList[0].Attributes[attrResult].Value;
            }
            return directoryName;
        }
        #endregion
    }

}

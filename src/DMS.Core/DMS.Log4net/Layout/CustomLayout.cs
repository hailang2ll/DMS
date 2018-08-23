using log4net.Core;
using log4net.Layout;
using log4net.Layout.Pattern;
using System.IO;
using System.Reflection;

namespace DMS.Log4net.Layout
{
    /// <summary>
    /// 
    /// </summary>
    public class LogMsgLayout : PatternLayout
    {
        /// <summary>
        /// 
        /// </summary>
        public LogMsgLayout()
        {
            AddConverter("property", typeof(LogMessagePatternConverter));
        }
    }

    /// <summary>
    /// 模式转换
    /// </summary>
    public class LogMessagePatternConverter : PatternLayoutConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="loggingEvent"></param>
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            if (Option != null)
            {
                WriteObject(writer, loggingEvent.Repository, LookupProperty(Option, loggingEvent));
            }
            else
            {
                WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
            }
        }

        /// <summary>
        /// 通过反射获取传入的日志对象的某个属性的值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="loggingEvent"></param>
        /// <returns></returns>
        private object LookupProperty(string property, LoggingEvent loggingEvent)
        {
            object propertyValue = string.Empty;
            PropertyInfo propertyInfo = loggingEvent.MessageObject.GetType().GetProperty(property);
            if (propertyInfo != null)
                propertyValue = propertyInfo.GetValue(loggingEvent.MessageObject, null);

            return propertyValue;
        }
    }
}
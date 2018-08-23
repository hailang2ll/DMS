using DMS.BaseFramework.Common.Helper;
using DMS.Log4net.Param;

namespace DMS.Log4net.Helper
{
    public class LogDBHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="subSysId"></param>
        /// <param name="subSysName"></param>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        public static void LogInfo(int userId, int subSysId, string subSysName, string message, int logType)
        {
            LogMessage logMessage = new LogMessage()
            {
                UserID = userId,
                Message = message,
                LogType = logType,
                SubSysID = subSysId,
                SubSysName = subSysName,
                IP = IPHelper.GetWebClientIp(),
                //Url = RequestHelper.GetRawUrl()
            };
            WriteLog(logMessage);
        }

        /// <summary>
        /// 写入日志统一入口
        /// </summary>
        /// <param name="msgLog"></param>
        private static void WriteLog(LogMessage msgLog)
        {
            Logger.WaLiuBasicInfo(msgLog);
        }
    }
}
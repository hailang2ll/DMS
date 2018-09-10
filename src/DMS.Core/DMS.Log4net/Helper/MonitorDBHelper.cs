using DMS.Log4net.Param;
using System;

namespace DMS.Log4net.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class MonitorDBHelper
    {
        /// <summary>
        /// 写入日志统一入口
        /// </summary>
        /// <param name="msgLog"></param>
        private static void WriteLog(MonitorMessage msgLog)
        {
            Logger.ApiMonitorInfo(msgLog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="time"></param>
        /// <param name="queryParams"></param>
        /// <param name="requesHead"></param>
        /// <param name="httpMethod"></param>
        public static void LogInfo(DateTime startTime, DateTime endTime, string time, string queryParams, string requesHead, string httpMethod)
        {
            MonitorMessage logMessage = new MonitorMessage()
            {
                //Url = RequestHelper.GetRawUrl(),
                StartTime = startTime,
                EndTime = endTime,
                Time = time,
                QueryParams = queryParams,
                RequesHead = requesHead,
                HttpMethod = httpMethod,
                IP = "",//IPHelper.GetWebClientIp(),
            };
            WriteLog(logMessage);
        }
    }
}
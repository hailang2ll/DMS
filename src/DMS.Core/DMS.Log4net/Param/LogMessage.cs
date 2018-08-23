namespace DMS.Log4net.Param
{
    /// <summary>
    /// 日志消息字段
    /// </summary>
    public class LogMessage
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 子系统ID
        /// </summary>
        public int SubSysID { get; set; }
        /// <summary>
        /// 子系统名称
        /// </summary>
        public string SubSysName { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// LogType
        /// </summary>
        public int LogType { get; set; }
    }
}
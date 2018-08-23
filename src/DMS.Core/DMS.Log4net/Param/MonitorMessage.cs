using System;

namespace DMS.Log4net.Param
{
    public class MonitorMessage
    {
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Time
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// QueryParams
        /// </summary>
        public string QueryParams { get; set; }

        /// <summary>
        /// Time
        /// </summary>
        public string RequesHead { get; set; }

        /// <summary>
        /// Time
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string HostNameUser { get; set; }
    }
}
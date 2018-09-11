using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.RabbitMQ.Models
{
    public class ResponseMQResult
    { /// <summary>
      /// 业务处理结果
      /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 业务数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 扩展数据
        /// </summary>
        public object ExtData { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 服务器处理时间
        /// </summary>
        public DateTime ServiceTime { get; set; } = DateTime.Now;
    }
}

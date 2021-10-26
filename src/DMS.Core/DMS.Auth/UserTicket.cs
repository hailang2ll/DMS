using System;

namespace DMS.Auth
{
    [Serializable]
    public class UserTicket
    {
        /// <summary>
        /// 用户唯一ID（业务ID）
        /// </summary>
        /// <value></value>
        public long ID { get; set; }
        /// <summary>
        /// 用户唯一code（业务code）
        /// </summary>
        public string EpCode { get; set; }
        /// <summary>
        /// 用户唯一标识（微信uid）
        /// </summary>
        public string UID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ExpDate { get; set; } = DateTime.Now.AddDays(90);
    }
}

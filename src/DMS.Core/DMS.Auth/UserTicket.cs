using System;

namespace DMS.Auth
{
    [Serializable]
    public class UserTicket
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        /// <value></value>
        public long ID { get; set; } = 0;
        /// <summary>
        /// 名称
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        public DateTime ExpDate { get; set; } = DateTime.Now.AddDays(90);
        public string Msg { get; set; } = "初始化";
        public int Code { get; set; }

        public string UnionID { get; set; }

    }
}

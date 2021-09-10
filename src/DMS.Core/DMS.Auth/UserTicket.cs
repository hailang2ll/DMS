using System;

namespace DMS.Auth
{
    [Serializable]
    public class UserTicket
    {
        public long ID { get; set; } = 0;
        public string Name { get; set; }
        public DateTime ExpDate { get; set; } = DateTime.Now.AddDays(90);
        public string Msg { get; set; } = "初始化";
        public int Code { get; set; }

        public string UnionID { get; set; }

    }
}

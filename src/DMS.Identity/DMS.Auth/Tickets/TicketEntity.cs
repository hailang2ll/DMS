using System;

namespace DMS.Auth.Tickets
{
    [Serializable]
    public class TicketEntity
    {
        public TicketEntity()
        {
            this.ID = "0";
            this.Name = "";
            this.ExpDate = DateTime.Now.AddDays(90);
            this.Msg = "初始化";
            this.Code = 0;

            this.UnionID = "";
        }
        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime ExpDate { get; set; }
        public string Msg { get; set; }
        public int Code { get; set; }

        public string UnionID { get; set; }
    }

}

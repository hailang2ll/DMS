using System;

namespace DMS.Redis.Tickets
{
    [Serializable]
    public class TicketEntity
    {
        public TicketEntity()
        {
            this.ID = "0";
            this.Name = "";
            this.Key = Guid.Empty;
            this.ExpDate = DateTime.Now;
            this.Msg = "初始化";
        }
        public string ID { get; set; }
        public string LinkKey { get; set; }
        public string Name { get; set; }
        public Guid Key { get; set; }
        public DateTime ExpDate { get; set; }
        public string Msg { get; set; }
    }

}

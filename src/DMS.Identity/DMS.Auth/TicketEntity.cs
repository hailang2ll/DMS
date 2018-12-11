using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Auth
{
    [Serializable]
    public class TicketEntity
    {
        public TicketEntity()
        {
            this.MemberID = 0;
            this.MemberName = "";
            this.MemberName = "";
            this.VisitorKey = Guid.Empty;
            this.UserKey = Guid.Empty;
            this.ExpDate = DateTime.Now;
        }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public string Msg { get; set; }
        public Guid? VisitorKey { get; set; }
        public Guid? UserKey { get; set; }
        public DateTime ExpDate { get; set; }
    }
}

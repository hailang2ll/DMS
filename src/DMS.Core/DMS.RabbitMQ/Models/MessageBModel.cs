using DMS.RabbitMQ.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.RabbitMQ.Models
{
    [RabbitMQ("DMS.QueueA", ExchangeName = "DMS.Exchange")]
    public class MessageBModel
    {
        public string Msg { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}

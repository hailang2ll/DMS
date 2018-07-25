using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.WebAPITest.Models
{
    public class TableConfigCollection
    {
        public string Name { get; set; }
        public string SqlType { get; set; }
        public bool WithLock { get; set; }
        public string Author { get; set; }
        public string ConnectString { get; set; }
    }
}

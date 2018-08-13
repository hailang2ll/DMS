using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.ConsoleAppTest.Model
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

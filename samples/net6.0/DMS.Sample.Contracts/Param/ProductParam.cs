using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Sample.Contracts.Param
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchProductParam
    {

        public int? ProductId { get; set; }

        public string ProductName { get; set; } 
            
    }
}

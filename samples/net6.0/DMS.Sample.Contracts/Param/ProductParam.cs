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
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "产品ID")]
        [Required(ErrorMessage = "{0}不能为空")]
        public int? ProductId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "产品名称")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(60, MinimumLength = 3)]
        public string ProductName { get; set; } 
            
    }
}

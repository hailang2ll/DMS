using DMS.Common.Model.Param;
using System.ComponentModel.DataAnnotations;

namespace DMS.IServices.Param
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchProductParam : PageParam
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int? ProductId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(8, ErrorMessage = "Name length can't be more than 8.")]
        public string ProductName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Range(0, 999.99)]
        public string ProductPrice { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.IServices.Result
{
    /// <summary>
    /// 产品返回实体
    /// </summary>
    public class ProductEntityResult
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 产品价格
        /// </summary>
        public decimal ProductPrice { get; set; }
        /// <summary>
        /// 产品创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
    }
}

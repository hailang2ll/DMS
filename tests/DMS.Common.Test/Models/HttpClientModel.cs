using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Common.Test.Models
{
    public class ProductEntityResult
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal? RetailPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ProductCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SkuAttrValueStr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SkuAttrValueIDs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SkuAttrIDs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProductDetail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? SpikePrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? BrandID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProductImages { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CategoryNamePath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CategoryCodePath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? CategoryID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ProductID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CountryImage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? SortOrder { get; set; }
    }

    /// <summary>
    /// 产品搜索
    /// </summary>
    public class SearchProductParam
    {
        /// <summary>
        /// 关键词
        /// </summary>
        public string SearchKey { get; set; }
        /// <summary>
        /// 发布人ID
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 属性集合
        /// </summary>
        public SearchProductAttrParam AttrParam { get; set; }
    }

    /// <summary>
    ///  私募股权，阳光私募，私募债权，信托，资管,银行理财
    /// </summary>
    public class SearchProductAttrParam
    {
        /// <summary>
        /// 产品分类
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// 产品销售状态
        /// </summary>
        public int? ProductStatusType { get; set; }

        /// <summary>
        /// 年化收益率
        /// </summary>
        public string IncomeRange1 { get; set; }

        /// <summary>
        /// 奖金费率
        /// </summary>
        public string BonusRatio { get; set; }

        /// <summary>
        /// 投资期限
        /// </summary>
        public string ProductTerm { get; set; }

        /// <summary>
        /// 付息方式
        /// </summary>
        public int? PaymentType { get; set; }

        /// <summary>
        /// 结算方式
        /// </summary>
        public int? BonusType { get; set; }

        /// <summary>
        /// 认购金额
        /// </summary>
        public string InvestmentStart { get; set; }

    }
}

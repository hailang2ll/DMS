using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Sample.Model
{
    public class ProductEntityResult
    {

        public decimal? RetailPrice { get; set; }
        public int? ProductCount { get; set; }
        public string SkuAttrValueStr { get; set; }
        public string SkuAttrValueIDs { get; set; }
        public string SkuAttrIDs { get; set; }
        public string ProductDetail { get; set; }
        public string BrandName { get; set; }
        public decimal? SpikePrice { get; set; }
        public int? BrandID { get; set; }
        public string CountryName { get; set; }
        public string ProductImages { get; set; }
        public string CategoryNamePath { get; set; }
        public string CategoryCodePath { get; set; }
        public int? CategoryID { get; set; }
        public string ProductName { get; set; }
        public int? ProductID { get; set; }
        public string CountryImage { get; set; }
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

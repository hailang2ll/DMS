using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DMS.Common.Test.Models.xml
{
    /// <summary>
    /// 订单查询
    /// </summary> 
    [Serializable]
    [XmlRoot("DOCUMENT")]
    public class DocumentResult
    {
        /// <summary>
        /// 交易返回码，成功时总为000000
        /// </summary>
        public string RETURN_CODE { get; set; }
        /// <summary>
        /// 交易返回提示信息，成功时为空
        /// </summary>
        public string RETURN_MSG { get; set; }

        /// <summary>
        /// 订单集合
        /// </summary>     
        [XmlElementAttribute(ElementName = "QUERYORDER")]
        public List<QUERYORDER> QUERYORDER { get; set; }
    }

    /// <summary>
    /// 订单集合
    /// </summary>  
    [Serializable]
    public class QUERYORDER
    {
        /// <summary>
        /// 商户代码
        /// </summary>
        public string MERCHANTID { get; set; }
        /// <summary>
        /// 商户所在分行
        /// </summary>
        public string BRANCHID { get; set; }
    }
}

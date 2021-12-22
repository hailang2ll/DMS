using DMS.Common.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DMS.Common.Test
{
    public enum EnumMemUserType
    {
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Description("普通用户")]
        Nornm = 1,
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Description("QQ用户")]
        QQType = 2,
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Description("dfdfdsfd ")]
        Test = 3
    }

    public enum EnumListBasicType
    {
        /// <summary>
        /// 汇总
        /// </summary>
        [System.ComponentModel.Description("汇总")]
        汇总 = 100,
        /// <summary>
        /// 已保存
        /// </summary>
        [System.ComponentModel.Description("综合单价组价明细")]
        综合单价组价明细 = 110,
        /// <summary>
        /// 综合单价分析表
        /// </summary>
        [System.ComponentModel.Description("综合单价分析表")]
        综合单价分析表 = 120,
        /// <summary>
        /// 主材
        /// </summary>
        [System.ComponentModel.Description("主材")]
        主材 = 130,
        /// <summary>
        /// 零星
        /// </summary>
        [System.ComponentModel.Description("零星")]
        零星 = 140,
        /// <summary>
        /// 措施
        /// </summary>
        [System.ComponentModel.Description("措施")]
        措施 = 150,
        /// <summary>
        /// 费率
        /// </summary>
        [System.ComponentModel.Description("费率")]
        费率 = 160,
        /// <summary>
        /// 增补清单
        /// </summary>
        [System.ComponentModel.Description("增补清单1")]
        增补清单 = 170,
    }

    /// <summary>
    /// 枚举扩展
    /// </summary>
    public class EnumExtensions_Test
    {

        public Dictionary<int, string> DicGroupRules
        {
            get
            {
                Dictionary<int, string> dic = new Dictionary<int, string>
                {
                    { 1, @"^[1-9]\d*$" },
                    { 2, @"^([1-9]\d*)(.)([1-9]\d*)$" },
                    { 3, @"^([1-9]\d*)(.)([1-9]\d*)(.)([1-9]\d*)$" },
                    { 4, @"^([1-9]\d*)(.)([1-9]\d*)(.)([1-9]\d*)(.)([1-9]\d*)$" },
                    { 5, @"^([1-9]\d*)(.)([1-9]\d*)(.)([1-9]\d*)(.)([1-9]\d*)(.)([1-9]\d*)$" }
                };
                return dic;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Test(Description = "")]
        public void EnumExtensions()
        {
            var json = typeof(EnumMemUserType).ToJson();
            var des = EnumMemUserType.QQType.GetDescription();
            des = typeof(EnumMemUserType).GetDescription(2);
            //var dics = typeof(EnumListBasicType).ToDictionaryKeyName();
            //var dicEntity = dics.Where(q => "综合单价组价明细".Contains(q.Key)).FirstOrDefault();

            var names = System.Enum.GetNames(typeof(EnumListBasicType));
            var values = System.Enum.GetValues(typeof(EnumListBasicType));

            var rule = DicGroupRules.Where(q => Regex.Match("aaa", q.Value).Success).FirstOrDefault();

            string tempcode = "001002";
            var t = tempcode.Substring(0, tempcode.Length - 3);

           
        }

      

    }

}

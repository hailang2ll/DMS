using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;

namespace DMS.Common.Helper
{
    /// <summary>
    /// excel 构建
    /// 仿照AutoMaper 调用方式
    /// 
    /// 
    /// 调用方式
    /// new ExportBuilderHelper<CustomerInsureModel>()
    ///         .Column(c => c.InsureNum)
    ///         .Column(c => c.Applicant)
    ///         .Column(c => c.Insurant)
    ///         .Column(c => c.CompanyName)
    ///         .Column(c => c.ProdName)
    ///         .Column(c => c.InsureTime, c => c.InsureTime.Value.ToString("yyyy-MM-dd HH:mm"))
    ///         .Column(c => c.IsMergePay, c => c.IsMergePay ? c.OrderNum : string.Empty)
    ///         .Column(c => c.BuySinglePrice, c => c.BuySinglePrice.Value.ToString("0.00"))
    ///         .Column(c => c.OnlinePaymnet, c => c.OnlinePaymnet.GetDescription())
    ///         .Export(vdata.ToList(), "用户信息.xls");
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExportBuilderHelper<T>
    {
        List<string> titles = new List<string>();
        List<Func<T, object>> funcs = new List<Func<T, object>>();

        /// <summary>
        /// 定义列
        /// 2012-08-15 支持 .Column(m => m.Platform01.ToString()) 写法
        /// </summary>
        /// <param name="member">解析Display特性得到列名，如不存在，则使用列名</param>
        /// <returns></returns>
        public ExportBuilderHelper<T> Column(Expression<Func<T, object>> member)
        {
            //var memberParam = member.Body as MemberExpression;
            titles.Add(GetDisplayName(member.Body));
            var convert = member.Compile();
            funcs.Add(convert);
            return this;
        }

        /// <summary>
        /// 定义列
        /// </summary>
        /// <param name="member"></param>
        /// <param name="title">列名</param>
        /// <returns></returns>
        public ExportBuilderHelper<T> Column(Expression<Func<T, string>> member, string title)
        {
            var memberParam = member.Body as MemberExpression;
            titles.Add(title);
            var convert = member.Compile();
            funcs.Add(convert);
            return this;
        }

        /// <summary>
        /// 定义列 
        /// </summary>
        /// <param name="member">解析Display特性得到列名，如不存在，则使用列名</param>
        /// <param name="convert">定义数据输出格式</param>
        /// <returns></returns>
        public ExportBuilderHelper<T> Column(Expression<Func<T, object>> member, Func<T, string> convert)
        {
            //var memberParam = member.Body as MemberExpression;
            titles.Add(GetDisplayName(member.Body));
            funcs.Add(convert);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <param name="title">列名</param>
        /// <param name="convert">定义数据输出格式</param>
        /// <returns></returns>
        public ExportBuilderHelper<T> Column(Expression<Func<T, object>> member, string title, Func<T, string> convert)
        {
            var memberParam = member.Body as MemberExpression;
            titles.Add(title);
            funcs.Add(convert);
            return this;
        }

        /// <summary>
        /// 编辑已经添加的转换
        /// </summary>
        /// <param name="name"></param>
        /// <param name="convert"></param>
        /// <returns></returns>
        public ExportBuilderHelper<T> Edit(string name, Func<T, string> convert)
        {
            var index = titles.FindIndex(m => m == name);
            if (index > -1)
            {
                funcs[index] = convert;
            }
            return this;
        }

        /// <summary>
        /// 构建
        /// </summary>
        /// <param name="list"></param>
        /// <param name="fileName"></param>
        public byte[] Export(List<T> list)
        {
            return ExportHelper.ExportToFsExcel<T>(list, titles.ToArray(), funcs.ToArray());
        }




        #region   
        /// <summary>
        /// [Display(Name = "")]
        /// 获得类属性中标记的名称
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        private static string GetDisplayName(Expression expr)
        {
            var memberParam = expr as MemberExpression;
            if (memberParam != null)
            {
                return GetDisplayName(memberParam);
            }
            var unary = expr as UnaryExpression;
            if (unary != null)
            {
                return GetDisplayName(unary.Operand as MemberExpression);
            }
            var call = expr as MethodCallExpression;
            if (call != null)
            {
                return GetDisplayName(call.Object as MemberExpression);
            }

            return string.Empty;

        }

        /// <summary>
        /// [Display(Name = "记住帐号")]
        /// 获得类属性中标记的中文名
        /// </summary>
        /// <param name="memberParam"></param>
        /// <returns></returns>
        private static string GetDisplayName(MemberExpression memberParam)
        {
            var name = memberParam.Member.Name;
            var property = memberParam.Member.ReflectedType.GetProperty(name);
            var displays = property.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (displays == null || displays.Length == 0)
                return property.Name;
            else
                return (displays[0] as DisplayAttribute).Name;
        }
        #endregion
    }
}

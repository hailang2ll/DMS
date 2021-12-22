using DMS.Common.Helper;
using DMS.Common.Model.Result;
using DMS.Common.Test.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DMS.Common.Test
{
    public class HttpWebHelper_Test
    {
        /// <summary>
        /// HttpWebHelper=>get请求
        /// </summary>
        /// <returns></returns>
        [Test(Description = "")]
        public void GetHttpWebHelper()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                { "SearchKey","国元信托"},
                { "AttrParam.CodeName","xintuo"},
                { "AttrParam.ProductStatusType","1"},
            };

            for (int i = 0; i <= 100; i++)
            {
                var result = HttpWebHelper.GetRequest<ResponseResult>("http://productapi.jinglih.com/api/Product/GetProductList", dict);
                Console.WriteLine(i + "====" + result);
            }
        }

        /// <summary>
        /// HttpWebHelper=>Post请求
        /// </summary>
        /// <returns></returns>
        [Test(Description = "")]
        public void PostHttpWebHelper()
        {
            SearchProductParam dict = new SearchProductParam()
            {
                SearchKey = "国元信托1",
                AttrParam = new SearchProductAttrParam() { CodeName = "xintuo", ProductStatusType = 1 },
            };
            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dict);

            for (int i = 0; i <= 100; i++)
            {
                var result = HttpWebHelper.PostRequest<ResponseResult>("http://productapi.jinglih.com/api/Product/GetProductList", jsonStr);
                Console.WriteLine(i + "====" + result);
            }
        }
    }

}

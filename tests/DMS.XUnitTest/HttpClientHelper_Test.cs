using DMS.Common.Helper;
using DMS.Common.Model.Result;
using DMS.XUnitTest.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace DMS.XUnitTest
{
    public class HttpClientHelper_Test
    {
        /// <summary>
        /// HttpClientHelper=>get请求
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName  = "")]
        public void GetHttpClientHelper()
        {
            string url = "https://productapi.trydou.com/api/product/GetProductEntity";
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                { "ProductID", "1123955947824353280" }
            };
            ResponseResult<ProductEntityResult> responseResult = HttpClientHelper.GetResponse<ResponseResult<ProductEntityResult>>(url, dic);
        }

        /// <summary>
        /// HttpClientHelper=>Post请求
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "")]
        public void PostHttpClientHelper()
        {
            SearchProductParam dict = new SearchProductParam()
            {
                SearchKey = "国元信托",
                AttrParam = new SearchProductAttrParam() { CodeName = "xintuo1" },
            };
            //第一种
            for (int i = 0; i <= 100; i++)
            {
                var result = HttpClientHelper.PostResponse<SearchProductParam>("http://productapi.jinglih.com/api/Product/GetProductList", dict);
                Console.WriteLine(i + "====" + result);
            }


            //第二种
            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dict);
            for (int i = 0; i <= 100; i++)
            {
                var result = HttpClientHelper.PostResponse("http://productapi.jinglih.com/api/Product/GetProductList", jsonStr);
                Console.WriteLine(i + "====" + result);
            }
            //第三种
            for (int i = 0; i <= 100; i++)
            {
                var result = HttpClientHelper.PostResponse<ResponseResult>("http://productapi.jinglih.com/api/Product/GetProductList", jsonStr);
                Console.WriteLine(i + "====" + result);
            }
        }
    }
}

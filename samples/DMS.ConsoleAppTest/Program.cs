using DMS.BaseFramework.Common.BaseParam;
using DMS.BaseFramework.Common.BaseResult;
using DMS.BaseFramework.Common.Helper;
using DMS.BaseFramework.Common.Serializer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.WebAPITest
{
    public class DataList
    {
        public List<ProductModel> PortalAdList { get; set; }
    }

    public class ProductModel
    {
        public string ConfigImage { get; set; }
        public string RelationLink { get; set; }
        public string ShowName { get; set; }
        public int LinkType { get; set; }
    }







    class Program
    {

        private static HashSet<string> set = new HashSet<string>();

        private static void GetID()
        {
            int num = 0;
            for (var i = 0; i < 1000000; i++)
            {
                var id = RandomHelper.CreateRandom(100000, 999999);//UniquenessHelper.GetWorkerID();

                if (set.Contains(id))
                {
                    num++;
                    Console.WriteLine("发现重复项 : {0}", id);
                }
                else
                {
                    Console.WriteLine("{0}=====重复总个数" + num, id);
                    set.Add(id);
                }

            }
            // Console.WriteLine($"任务{++taskCount}完成");
        }


        static void Main(string[] args)
        {
            //Get();
            //Post();
            //Post1();
            //Post2();

            //HttpWebHelper_Post();
            //HttpWebHelper_Get();

            Task.Run(() => GetID());

            Console.ReadKey();
        }


        static void Get()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                { "SearchKey","国元信托"},
                { "AttrParam.CodeName","xintuo"},
                { "AttrParam.ProductStatusType","1"},
            };
            for (int i = 0; i <= 100; i++)
            {
                var result = HttpClientHelper.GetResponse<ResponseResult<DataList>>("http://productapi.jinglih.com/api/Product/GetProductList", dict);
                Console.WriteLine(i + "====" + SerializerJson.SerializeObject(result));
            }
        }

        static void Post()
        {
            //Dictionary<string, string> dict = new Dictionary<string, string>()
            //{
            //    { "SearchKey","国元信托"},
            //    { "AttrParam.CodeName","xintuo"},
            //    { "AttrParam.ProductStatusType","1"},
            //};
            SearchProductParam dict = new SearchProductParam()
            {
                SearchKey = "国元信托",
                AttrParam = new SearchProductAttrParam() { CodeName = "xintuo1" },
            };


            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dict);
            for (int i = 0; i <= 100; i++)
            {
                var result = HttpClientHelper.PostResponse("http://productapi.jinglih.com/api/Product/GetProductList", jsonStr);
                Console.WriteLine(i + "====" + result);
            }
        }

        static void Post1()
        {
            SearchProductParam dict = new SearchProductParam()
            {
                SearchKey = "国元信托",
                AttrParam = new SearchProductAttrParam() { CodeName = "xintuo2" },
            };

            for (int i = 0; i <= 100; i++)
            {
                var result = HttpClientHelper.PostResponse<SearchProductParam>("http://productapi.jinglih.com/api/Product/GetProductList", dict);
                Console.WriteLine(i + "====" + result);
            }
        }

        static void Post2()
        {
            SearchProductParam dict = new SearchProductParam()
            {
                SearchKey = "国元信托",
                AttrParam = new SearchProductAttrParam() { CodeName = "xintuo3" },
            };
            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dict);

            for (int i = 0; i <= 100; i++)
            {
                var result = HttpClientHelper.PostResponse<ResponseResult>("http://productapi.jinglih.com/api/Product/GetProductList", jsonStr);
                Console.WriteLine(i + "====" + result);
            }
        }

        static void HttpWebHelper_Post()
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

        static void HttpWebHelper_Get()
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
    }



    #region 产品列表+搜索
    /// <summary>
    /// 产品搜索
    /// </summary>
    public class SearchProductParam : PageParam
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

    #endregion
}

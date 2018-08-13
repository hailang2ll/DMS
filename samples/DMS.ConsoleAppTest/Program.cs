using DMS.BaseFramework.Common;
using DMS.BaseFramework.Common.BaseParam;
using DMS.BaseFramework.Common.BaseResult;
using DMS.BaseFramework.Common.Configuration;
using DMS.BaseFramework.Common.Helper;
using DMS.BaseFramework.Common.Serializer;
using DMS.ConsoleAppTest.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DMS.WebAPITest
{

    class Program
    {
        private static HashSet<long> longSet = new HashSet<long>();
        private static int taskCount = 0;

        static void Main(string[] args)
        {
            ConfigurationDemo();
            DomainManageSettingsDemo();
            //序列化
            SerializerJsonDemo();
            SerializerXmlDemo();


            HttpClientHelperGetDemo();
            HttpClientHelperPostDemo();

            HttpWebHelperGet();
            HttpWebHelperPost();

            Task.Run(() => GetNewID());
            Task.Run(() => GetNewID());
            Task.Run(() => GetNewID());
            Task.Run(() => Printf());

            Console.ReadKey();
        }


        static void ConfigurationDemo()
        {
            //获取appsettings.json的配置目录
            var configPath = AppSettings.GetConfigPath;
            var tableConfigCollection = AppSettings.GetModel<List<TableConfigCollection>>("TableConfigCollection");
            //读取指定的目录(configPath)中的TableConfig.json文件
            var tableConfig = AppSettings.GetCustomModel<List<TableConfigCollection>>("TableConfig.json", "TableConfigurations");
        }
        static void DomainManageSettingsDemo()
        {
            //读取指定的目录(configPath)Domain.json文件
            var ImgServerUrl = DomainManageSettings.PortalUrl;
        }

        #region Serializer
        /// <summary>
        /// 序列化
        /// </summary>
        static void SerializerJsonDemo()
        {
            SearchProductParam dict = new SearchProductParam()
            {
                SearchKey = "国元信托1",
                AttrParam = new SearchProductAttrParam() { CodeName = "xintuo", ProductStatusType = 1 },
            };
            var jsonStr = SerializerJson.SerializeObject(dict);

            dict = SerializerJson.DeserializeObject<SearchProductParam>(jsonStr);
        }
        static void SerializerXmlDemo()
        {
            //需要添加实例
        }
        #endregion

        #region HttpClientHelper
        static void HttpClientHelperGetDemo()
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

        static void HttpClientHelperPostDemo()
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
        #endregion

        #region HttpWebHelper
        static void HttpWebHelperGet()
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

        static void HttpWebHelperPost()
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
        #endregion

        #region GetNewID
        private static object o = new object();
        private static int N = 20000;
        static void GetNewID()
        {
            int num = 0;
            for (var i = 0; i < N; i++)
            {
                var id = UniquenessHelper.GetWorkerID();
                lock (o)
                {
                    if (longSet.Contains(id))
                    {
                        num++;
                        Console.WriteLine("发现重复项 : {0}", id);
                    }
                    else
                    {
                        longSet.Add(id);
                    }
                }

            }
            Console.WriteLine($"任务{++taskCount}完成");
        }
        private static void Printf()
        {
            while (taskCount != 3)
            {
                Console.WriteLine("...");
                Thread.Sleep(1000);
            }
            Console.WriteLine(longSet.Count == N * taskCount);
        }
        #endregion

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

    #region 广告实体
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
    #endregion

}

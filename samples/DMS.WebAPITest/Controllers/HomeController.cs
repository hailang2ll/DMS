using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exceptionless;
using DMS.BaseFramework.Common.Helper;
using System.Data;
using DMS.BaseFramework.Common.Configuration;
using DMS.BaseFramework.Common.Extension;
using DMS.Redis;
using DMS.WebAPITest.Models;

//[assembly: Exceptionless("aMFZxvXEs0kWTR3gvfOsjMcwrHJcFHN7XSRNV65U", ServerUrl = "http://192.168.0.56:9002")]
namespace DMS.BaseFramework.Common.APITest.Controllers
{

    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            var configPath = AppSettings.GetConfigPath;
            var tableConfigCollection = AppSettings.GetModel<List<TableConfigCollection>>("TableConfigCollection");

            var RedisEntity = AppSettings.GetCustomModel<RedisEntityConfig>("Redis.json");
            var tableConfig = AppSettings.GetCustomModel<List<TableConfigCollection>>("TableConfig.json", "TableConfigurations");


            #region 分布式日志收集
            //LessLog.Info("这是一条提示信息");
            //LessLog.Error("这是一条错误的信息");
            //LessLog.Fatal("这是一条致命的信息");

            //try
            //{
            //    var a = "aaa";
            //    Convert.ToInt32(a);
            //}
            //catch (Exception ex)
            //{
            //    LessLog.Exception("异常信息aa", ex);
            //}
            #endregion

            #region redis缓存
            //RedisManager redisManager = new RedisManager(0);
            //redisManager.StringSet("key", "value1");
            //var key = redisManager.StringGet("key");
            #endregion


            var ImgServerUrl = DomainManageSettings.GetDomain("ImgServerUrl");

            return View();
        }

        #region excel导出，只支持web
        /// <summary>
        /// excel导出
        /// </summary>
        /// <returns></returns>
        public FileResult ExportFS()
        {
            List<User> userList = new List<User>() {
                 new User(){ ID=1,Name="aaa" },
                 new User(){ ID=2,Name="bbb" },
            };
            var fs = ExportHelper.ExportToFsExcel<User>(userList, new string[] { "ID", "Name" },
               c => c.ID,
               c => c.Name.ToString()
               );

            return File(fs, "application/vnd.ms-excel", "ExportFS");
        }

        /// <summary>
        /// excel导出
        /// </summary>
        /// <returns></returns>
        public FileResult ExportBuilderFS()
        {
            List<User> userList = new List<User>() {
                 new User(){ ID=1,Name="aaa" },
                 new User(){ ID=2,Name="bbb" },
            };

            var fs = new ExportBuilderHelper<User>()
                   .Column(c => c.ID)
                   .Column(c => c.Name)
                   .Export(userList);

            return File(fs, "application/vnd.ms-excel", "ExportBuilderFS");
        }
        #endregion

        #region excel导出，支持所有方式导出，推荐用这两种方式
        /// <summary>
        /// API导出数据
        /// </summary>
        /// <returns></returns>
        public string Export()
        {
            List<User> list = new List<User>();
            list.Add(new User() { ID = 1, Name = "lang" });
            list.Add(new User() { ID = 1, Name = "aaa" });
            DataTable dt = list.ToDataTable("dd");
            return ExportHelper.ExportToExcel(dt, @"d:\Export.xls");
        }
        public string ExportToExcel()
        {
            List<User> userList = new List<User>() {
                 new User(){ ID=1,Name="aaa" },
                 new User(){ ID=2,Name="bbb" },
            };
            var s = ExportHelper.ExportToExcel<User>(userList, @"d:\ExportToExcel.xls",
               new string[] { "用户ID", "用户名称" },
               c => c.ID,
               c => c.Name.ToString()
               );

            return s;
        }
        #endregion

        #region 分布日志
        public static void LessLog1()
        {

            ExceptionlessClient.Default.Configuration.ApiKey = "aMFZxvXEs0kWTR3gvfOsjMcwrHJcFHN7XSRNV65U";
            ExceptionlessClient.Default.Configuration.ServerUrl = "http://192.168.0.56:9002";

            try
            {
                throw new Exception("这是分布式日志收集 LiYouMing项目QQQQ");
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
            }

            // 发送日志
            ExceptionlessClient.Default.SubmitLog("Logging made easy");

            // 你可以指定日志来源，和日志级别。
            // 日志级别有这几种: Trace, Debug, Info, Warn, Error
            ExceptionlessClient.Default.SubmitLog(typeof(HomeController).FullName, "This is so easy", "Info");
            ExceptionlessClient.Default.CreateLog(typeof(HomeController).FullName, "This is so easy", "Info").AddTags("Exceptionless").Submit();


            // 发送 Feature Usages
            ExceptionlessClient.Default.SubmitFeatureUsage("MyFeature");
            ExceptionlessClient.Default.CreateFeatureUsage("MyFeature").AddTags("Exceptionless").Submit();

            // 发送一个 404
            ExceptionlessClient.Default.SubmitNotFound("/somepage");
            ExceptionlessClient.Default.CreateNotFound("/somepage").AddTags("Exceptionless").Submit();
        }
        public static void LessLog2()
        {
            var client = new ExceptionlessClient(c =>
            {
                c.ApiKey = "aMFZxvXEs0kWTR3gvfOsjMcwrHJcFHN7XSRNV65U";
                c.ServerUrl = "http://192.168.0.56:9002";
            });


            try
            {
                throw new Exception("这是分布式日志收集 WWWW");
            }
            catch (Exception ex)
            {
                client.SubmitException(ex);
            }
        }

        /// <summary>
        /// 未成功， 是否需要生产域名，需要验证
        /// </summary>
        public static void LessLog3()
        {
            ExceptionlessClient.Default.Startup("aMFZxvXEs0kWTR3gvfOsjMcwrHJcFHN7XSRNV65U");

            try
            {
                throw new Exception("这是分布式日志收集 eeeee");
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
            }
        }
        #endregion

















        public IActionResult About()
        {

            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

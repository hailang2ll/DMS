using DMS.WebAPITest.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

//[assembly: Exceptionless("aMFZxvXEs0kWTR3gvfOsjMcwrHJcFHN7XSRNV65U", ServerUrl = "http://192.168.0.56:9002")]
namespace DMS.BaseFramework.Common.APITest.Controllers
{

    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            #region redis缓存
            //RedisManager redisManager = new RedisManager(0);
            //redisManager.StringSet("key", "value1");
            //var key = redisManager.StringGet("key");
            #endregion
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult About()
        {

            ViewData["Message"] = "Your application description page.";

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> TestLog()
        {
            var ex = new ArgumentNullException("空异常");
            Log4net.Logger.Info($"Log4net消息日志：{Guid.NewGuid().ToString("N")}", ex);
            Log4net.Logger.Warn($"Log4net警告日志：{Guid.NewGuid().ToString("N")}", ex);
            Log4net.Logger.Error($"Log4net异常日志：{Guid.NewGuid().ToString("N")}", ex);

            NLog.NLogger.Debug($"NLog调试日志：{Guid.NewGuid().ToString("N")}", ex);
            NLog.NLogger.Info($"NLog消息日志：{Guid.NewGuid().ToString("N")}", ex);
            NLog.NLogger.Warn($"NLog警告日志：{Guid.NewGuid().ToString("N")}", ex);
            NLog.NLogger.Error($"NLog异常日志：{Guid.NewGuid().ToString("N")}", ex);
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
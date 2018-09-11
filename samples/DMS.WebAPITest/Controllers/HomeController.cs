using DMS.Log4net;
using DMS.NLogs;
using DMS.Redis;
using DMS.WebAPITest.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace DMS.BaseFramework.Common.APITest.Controllers
{

    public class HomeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult<string> LogTest()
        {
            try
            {
                Logger.Debug($"Log4net调试日志：{Guid.NewGuid().ToString("N")}");
                Logger.Info($"Log4net消息日志：{Guid.NewGuid().ToString("N")}");
                Logger.Warn($"Log4net警告日志：{Guid.NewGuid().ToString("N")}");

                NLogger.Debug($"NLog调试日志：{Guid.NewGuid().ToString("N")}");
                NLogger.Info($"NLog消息日志：{Guid.NewGuid().ToString("N")}");
                NLogger.Warn($"NLog警告日志：{Guid.NewGuid().ToString("N")}");
                throw new NullReferenceException("空异常");
            }
            catch (Exception ex)
            {
                Logger.Error($"Log4net异常日志：{Guid.NewGuid().ToString("N")}", ex);
                NLogger.Error($"NLog异常日志：{Guid.NewGuid().ToString("N")}", ex);
            }
            return "";
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
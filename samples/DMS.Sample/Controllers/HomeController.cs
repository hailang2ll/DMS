using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Log4net;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Sample.Controllers
{
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

            DMS.Log4net.Logger.Info("Index=>这是log4net的日志");
            DMS.Log4net.Logger.Error("Index=>这是log4net的异常日志");

            DMS.NLogs.Logger.Info("Index=>这是nlog的日志");
            DMS.NLogs.Logger.Error("Index=>这是nlog的异常日志");
            return Ok("我是DMS首页");
        }
    }
}
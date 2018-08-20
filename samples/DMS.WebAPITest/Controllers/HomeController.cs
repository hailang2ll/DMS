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

            #region redis缓存
            //RedisManager redisManager = new RedisManager(0);
            //redisManager.StringSet("key", "value1");
            //var key = redisManager.StringGet("key");
            #endregion



            return View();
        }




















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

using Microsoft.AspNetCore.Mvc;

namespace DMS.Api.Controllers
{
    /// <summary>
    /// 我是MVC接口
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var murl = DMS.Common.AppConfig.GetValue("MemberUrl");
            return Content("OK-" + murl);
        }

      
    }

   
}

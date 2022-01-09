using Microsoft.AspNetCore.Mvc;

namespace DMS.Admin.WebSite.Controllers
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
            return Redirect("/login.html");
            //return Content("我是正常的");
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace DMS.Sample.Api.Controllers
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
        public IActionResult Index() => Content("OK");
    }
}

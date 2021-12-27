using Microsoft.AspNetCore.Mvc;

namespace DMS.Sample.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CmsUserController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}

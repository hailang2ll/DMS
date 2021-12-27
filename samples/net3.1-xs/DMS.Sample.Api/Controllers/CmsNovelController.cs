using Microsoft.AspNetCore.Mvc;

namespace DMS.Sample.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CmsNovelController : Controller
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

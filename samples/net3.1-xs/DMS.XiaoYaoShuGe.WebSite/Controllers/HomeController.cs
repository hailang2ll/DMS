using DMS.Sample.Contracts;
using DMS.Sample.Entity;
using GenFu;
using Microsoft.AspNetCore.Mvc;

namespace DMS.XiaoYaoShuGe.WebSite.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            ViewData["Message"] = "Your application description page.";
            var data = GenFu.GenFu.ListOf<CmsUser>(10);
            return View(data);
        }
    }
}

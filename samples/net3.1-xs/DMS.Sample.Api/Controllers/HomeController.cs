using DMS.Sample.Contracts;
using GenFu;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Sample.Api.Controllers
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
            var data = GenFu.GenFu.ListOf<UserEntity>(10);
            return View(data);
        }
    }
}

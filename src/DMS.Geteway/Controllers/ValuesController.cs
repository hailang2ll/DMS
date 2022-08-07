using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Geteway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetNew")]
        public IActionResult GetNew(int id)
        {
            DMS.NLogs.Logger.Info("aaaaa" + id);
            return Content("aaa-" + id);


        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddNew")]
        public IActionResult AddNew(AddNewParam param)
        {
            DMS.NLogs.Logger.Info("ddddddddddddddddd");
            return Content("OK-");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AddNewParam
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }
}

using DMS.Common.ControllerExt;
using DMS.Common.Model.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Service1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Hello")]
        public IActionResult Hello()
        {
            return Content("我是user1");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetUser")]
        public ResponseResult GetUser(long id)
        {
            ResponseResult result = new ResponseResult()
            {
                data = id,
                errmsg = "我是user1",
            };
            return result;
        }
    }
}

using DMS.Common.Model.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Api2.Controllers
{
    /// <summary>
    /// user
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Permission")]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// GetUser
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetUser")]
        public async Task<ResponseResult> GetUser(long id)
        {
            ResponseResult result = new ResponseResult();
            return await Task.FromResult(result);
        }
    }
}

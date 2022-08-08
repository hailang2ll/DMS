using DMS.Common.Model.Result;
using DMS.Extensions.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CookiesController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICookieHelper _cookieHelper;
        /// <summary>
        /// 
        /// </summary>
        private readonly DMS.Authorizations.UserContext.Cookies.IUserAuth _userAuth;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookieHelper"></param>
        /// <param name="userAuth"></param>
        public CookiesController(ICookieHelper cookieHelper, DMS.Authorizations.UserContext.Cookies.IUserAuth userAuth)
        {
            _cookieHelper = cookieHelper;
            _userAuth= userAuth;
        }
        /// <summary>
        /// CookiesLogin登录
        /// </summary>
        /// <returns></returns>
        [HttpPost("CookiesLogin")]
        public async Task<ResponseResult> CookiesLogin()
        {
            ResponseResult result = new();
            _cookieHelper.SetCookie("ID", "1");
            _cookieHelper.SetCookie("UserName", "hailang");
            return await Task.FromResult(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCookies")]
        public async Task<ResponseResult> GetCookies()
        {
            ResponseResult result = new();
            var id = _userAuth.ID;
            var userName = _userAuth.Name;

            result.data = new
            {
                id,
                userName
            };
            return await Task.FromResult(result);
        }
    }
}

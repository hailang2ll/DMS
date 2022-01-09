using DMS.Common.Encrypt;
using DMS.Common.Model.Result;
using DMS.Extensions.Authorizations;
using DMS.Sample.Contracts;
using DMS.Sample.Contracts.Param;
using DMS.Sample.Contracts.Result;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.Admin.WebSite.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ICmsUserService cmsUserService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmsUserService"></param>
        public LoginController(ICmsUserService cmsUserService)
        {
            this.cmsUserService = cmsUserService;
        }
        /// <summary>
        /// 我是API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Content("我是API");
        }


        #region 业务接口
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost("DoLogin")]
        public async Task<ResponseResult<LoginCmsUserResult>> DoLogin(LoginCmsUserParam param)
        {
            ResponseResult<LoginCmsUserResult> result = await cmsUserService.Login(param);
            if (result.errno == 0)
            {
                //写入cookies
                CookieOptions options = new CookieOptions()
                {
                    HttpOnly = true,
                    Domain = "",
                };
                HttpContext.Response.Cookies.Append("UserID", EncryptHelper.Encrypt(result.data.Id.ToString()), options);
                HttpContext.Response.Cookies.Append("UserName", EncryptHelper.Encrypt(result.data.UserName.ToString()), options);
            }
            return result;
        }
        #endregion
    }
}

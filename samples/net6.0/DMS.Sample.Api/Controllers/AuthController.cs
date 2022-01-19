using DMS.Common.Helper;
using DMS.Common.Model.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMS.Sample.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "user")]
    //[Authorize(Roles = "admin,user")]
    //[Authorize(Policy = "BaseRole")]
    //[Authorize(Policy = "MoreBaseRole")]
    //[Authorize(Roles = "dylan")]
    //[Authorize(Roles ="system")]
    //[Authorize(Policy = "customizePermisson")]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="userToken"></param>
        /// <param name="userOauth"></param>

        public AuthController(DMS.Auth.Token.IUserAuth userToken, DMS.Auth.Oauth2.IUserAuth userOauth)
        {
            this.userToken = userToken;
            this.userOauth = userOauth;
        }
        /// <summary>
        /// 
        /// </summary>
        private readonly DMS.Auth.Token.IUserAuth userToken;
        /// <summary>
        /// 
        /// </summary>
        private readonly DMS.Auth.Oauth2.IUserAuth userOauth;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("CheckAuth")]
        //[TypeFilter(typeof(CheckLoginAttribute))]
        public async Task<ResponseResult> CheckAuth()
        {
            var id = userToken.ID;
            var name = userToken.Name;
            var token = userToken.GetToken();

            var isAuth = userOauth.IsAuthenticated();
            var id2 = userOauth.ID2;
            var name2 = userOauth.Name2;
            var a = DMS.Common.AppConfig.GetValue("AllowedHosts");
            var ip = IPHelper.GetCurrentIp();
            var (loginFlag, result) = await userOauth.ChenkLoginAsync();
            if (!loginFlag)
            {
                return result;
            }

            return new ResponseResult() { data = new { isAuth, id2, name2 } };
        }


    }
}

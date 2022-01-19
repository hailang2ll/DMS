using DMS.Auth;
using DMS.Common.JsonHandler;
using DMS.Common.Model.Result;
using DMS.Extensions.Authorizations;
using DMS.Redis;
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

namespace DMS.Sample.Api.Controllers
{
    /// <summary>
    /// 我是登录
    /// 颁发令牌
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IRedisRepository redisRepository;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="redisRepository"></param>
        public LoginController(IRedisRepository redisRepository)
        {
            this.redisRepository = redisRepository;
        }
        /// <summary>
        /// 普通TOKEN认识方式
        /// </summary>
        /// <returns></returns>
        [HttpPost("TokenLogin")]
        public async Task<ResponseResult> TokenLogin()
        {
            ResponseResult result = new ResponseResult();
            UserTicket tokenModel = new UserTicket
            {
                ID = 120,
                EpCode = "100214545454",
                UID = "435353534",
                ExpDate = DateTime.Now.AddDays(1),
            };
            string sid = DMS.Extensions.UniqueGenerator.UniqueHelper.GetWorkerID().ToString();
            await redisRepository.SetAsync(sid, tokenModel.SerializeObject(), tokenModel.ExpDate);
            result.data = sid;
            return result;
        }

        /// <summary>
        /// JWT认证
        /// </summary>
        /// <returns></returns>
        [HttpPost("Login")]
        public ResponseResult Login()
        {
            ResponseResult result = new ResponseResult();

            var list = new List<dynamic> {
                new { Id="12", UserName="aaa",Pwd="123456",Role="admin"},
                new { Id="45", UserName="bbb",Pwd="456789",Role="system"},
            };

            var user = list.SingleOrDefault(q => q.UserName == "aaa" && q.Pwd == "123456");
            if (user == null)
            {
                result.errno = 1;
                result.errmsg = "用户名或密码错误";
                return result;
            }
            else
            {
                TokenModelJwt tokenModel = new TokenModelJwt
                {
                    Uid = 120,
                    Name = "dylan-123",
                    Role = "dylan",
                    Work = "dev",
                };
                result.data = JwtHelper.IssueJwt(tokenModel);
            }
            return result;
        }

        /// <summary>
        /// 主方法认证
        /// </summary>
        /// <returns></returns>
        [HttpPost("MainLogin")]
        public ResponseResult MainLogin()
        {
            ResponseResult result = new ResponseResult();

            var list = new List<dynamic> {
                new { Id="12", UserName="aaa",Pwd="123456",Role="admin"},
                new { Id="45", UserName="bbb",Pwd="456789",Role="system"},
            };

            var user = list.SingleOrDefault(q => q.UserName == "aaa" && q.Pwd == "123456");
            if (user == null)
            {
                result.errno = 1;
                result.errmsg = "用户名或密码错误";
                return result;
            }
            else
            {
                var userRoles = list.Select(q => q.Role).ToList();
                //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, user.Id),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(TimeSpan.FromSeconds(60 * 60).TotalSeconds).ToString()) };
                claims.AddRange(userRoles.Select(s => new Claim(ClaimTypes.Role, s)));
                var token = JwtHelper.BuildJwtToken(claims.ToArray());

                result.data = token;
            }
            return result;
        }

        /// <summary>
        /// LoginByCookies 认证
        /// </summary>
        /// <returns></returns>
        [HttpPost("LoginByCookies")]
        public ResponseResult LoginByCookies()
        {
            ResponseResult result = new ResponseResult();

            var list = new List<dynamic> {
                new { Id="12", UserName="aaa",Pwd="123456",Role="admin"},
                new { Id="45", UserName="bbb",Pwd="456789",Role="system"},
            };

            var user = list.SingleOrDefault(q => q.UserName == "aaa" && q.Pwd == "123456");
            if (user == null)
            {
                result.errno = 1;
                result.errmsg = "用户名或密码错误";
                return result;
            }
            else
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);//一定要声明AuthenticationScheme
                identity.AddClaim(new Claim(ClaimTypes.PrimarySid, user.Id));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Sid, user.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));
                HttpContext.SignInAsync(identity.AuthenticationType, new ClaimsPrincipal(identity));

                return result;
            }

        }
    }
}

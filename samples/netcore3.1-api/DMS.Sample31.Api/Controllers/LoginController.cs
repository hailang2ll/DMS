using DMS.Extensions.Authorizations;
using DMSN.Common.BaseResult;
using DMSN.Common.Encrypt;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DMS.Sample31.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetToken")]
        public IActionResult GetToken()
        {
            try
            {

                //定义发行人issuer
                string iss = "JWTBearer.Auth";
                //定义受众人audience
                string aud = "api.auth";

                //定义许多种的声明Claim,信息存储部分,Claims的实体一般包含用户和一些元数据
                IEnumerable<Claim> claims = new Claim[]
                {
                    new Claim(JwtClaimTypes.Id,"1"),
                    new Claim(JwtClaimTypes.Name,"i3yuan"),
                    new Claim(JwtClaimTypes.Role,"admin"),
                     new Claim(JwtClaimTypes.Role,"user"),
                };

                //expires   //过期时间
                // long Exp = new DateTimeOffset(DateTime.Now.AddSeconds(1000)).ToUnixTimeSeconds();
                var Exp = DateTime.UtcNow.AddSeconds(1000);
                //signingCredentials  签名凭证
                string sign = "q2xiARx$4x3TKqBJ"; //SecurityKey 的长度必须 大于等于 16个字符
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sign));
                var signcreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                //String issuer = default(String), String audience = default(String), IEnumerable<Claim> claims = null, Nullable<DateTime> notBefore = default(Nullable<DateTime>), Nullable<DateTime> expires = default(Nullable<DateTime>), SigningCredentials signingCredentials = null

                var jwt = new JwtSecurityToken(
                    issuer: iss,
                    audience: aud,
                    claims: claims,
                    expires: Exp,
                    signingCredentials: signcreds);

                var JwtHander = new JwtSecurityTokenHandler();

                var token = JwtHander.WriteToken(jwt);

                return Ok(new
                {
                    access_token = token,
                    token_type = "Bearer",
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("Login")]
        public IActionResult Login()
        {
            string jwtStr = string.Empty;
            bool suc = false;

            string name = "dylan";
            string pass = "123456";
            //pass = MD5Helper.MD5Crypto32(pass);

            //业务逻辑处理
            if (name == "dylan" && pass == "123456")
            {
                TokenModelJwt tokenModel = new TokenModelJwt
                {
                    Uid = 120,
                    Name = "dylan-123",
                    Role = "Admin",
                    Work = "dev",
                };

                jwtStr = JwtHelper.IssueJwt(tokenModel);
                suc = true;
            }
            else
            {
                jwtStr = "login fail!!!";
            }
            var result = new
            {
                success = suc,
                token = jwtStr
            };
            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("Login2")]
        public IActionResult Login2()
        {
            string jwtStr = string.Empty;
            bool suc = false;

            string name = "dylan";
            string pass = "123456";
            //pass = MD5Helper.MD5Crypto32(pass);

            //业务逻辑处理
            //if (name == "dylan" && pass == "123456")
            //{
            var userRoles = "dylan,admin";
            //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
            var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(JwtRegisteredClaimNames.Jti, "12"),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(TimeSpan.FromSeconds(60 * 60).TotalSeconds).ToString()) };
            claims.AddRange(userRoles.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

            var token = JwtHelper.BuildJwtToken(claims.ToArray());
            suc = true;
            //}
            //else
            //{
            //    jwtStr = "login fail!!!";
            //}
            var result = new
            {
                success = suc,
                token = token
            };
            return Ok(result);
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

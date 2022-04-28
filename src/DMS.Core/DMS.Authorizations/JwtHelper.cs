using DMS.Authorizations.Model;
using DMS.Authorizations.Policys;
using DMS.Authorizations.Result;
using DMS.Authorizations.UserContext.Dto;
using DMS.Common.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DMS.Authorizations
{
    public class JwtHelper
    {
        //public static string ValidAudience = "aaaa"+ DateTime.Now.ToString();
        /// <summary>
        /// 
        /// </summary>
        private readonly PermissionRequirement _requirement;
        public JwtHelper(PermissionRequirement requirement)
        {
            _requirement = requirement;
        }
        /// <summary>
        /// 获取基于JWT的Token
        /// </summary>
        /// <param name="claims">需要在登陆的时候配置</param>
        /// <param name="permissionRequirement">在startup中定义的参数</param>
        /// <returns></returns>
        public static TokenInfoResult Create(Claim[] claims, PermissionRequirement permissionRequirement)
        {
            var thisTime = DateTime.Now;
            DateTime expiresTime = thisTime.Add(permissionRequirement.Expiration);

            // 实例化JwtSecurityToken
            var jwt = new JwtSecurityToken(
                issuer: permissionRequirement.Issuer,
                audience: permissionRequirement.Audience,
                claims: claims,
                notBefore: thisTime,
                expires: expiresTime,
                signingCredentials: permissionRequirement.SigningCredentials
            );
            // 生成 Token
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            //打包返回前台
            var responseJson = new TokenInfoResult
            {
                success = true,
                token = encodedJwt,
                expires = expiresTime,
                token_type = "Bearer"
            };
            return responseJson;
        }

        /// <summary>
        /// 创建TOKEN
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static TokenInfoResult Create(Claim[] claims)
        {
            #region 参数
            var thisTime = DateTime.Now;
            JwtSettingModel option = DMS.Common.AppConfig.GetValue<JwtSettingModel>("JwtSetting");
            string issuer = option.Issuer;
            string audience = option.Audience;
            string secretCredentials = option.SecretKey;
            double expireMinutes = option.ExpireMinutes;

            var keyByteArray = Encoding.ASCII.GetBytes(secretCredentials);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            DateTime expiresTime = thisTime.AddMinutes(expireMinutes);
            #endregion

            // 实例化JwtSecurityToken
            var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: thisTime,
                expires: expiresTime,
                signingCredentials: signingCredentials
            );
            // 生成 Token
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            //打包返回前台
            var responseJson = new TokenInfoResult
            {
                success = true,
                token = encodedJwt,
                expires = expiresTime,
                token_type = "Bearer"
            };
            return responseJson;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static TokenInfoResult Create(UserClaimModel model)
        {
            #region 参数
            var thisTime = DateTime.Now;
            JwtSettingModel option = DMS.Common.AppConfig.GetValue<JwtSettingModel>("JwtSetting");
            string issuer = option.Issuer;
            string audience = option.Audience;
            string secretCredentials = option.SecretKey;
            double expireMinutes = option.ExpireMinutes;

            var keyByteArray = Encoding.ASCII.GetBytes(secretCredentials);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            DateTime expiresTime = thisTime.AddMinutes(expireMinutes);
            #endregion


            var claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Uid, model.Uid),
                    new Claim(JwtClaimTypes.Cid, model.Cid),
                    new Claim(JwtClaimTypes.EpCode, model.EpCode),
                    new Claim(JwtClaimTypes.Expiration, model.Expiration)
                };


            // 实例化JwtSecurityToken
            var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: thisTime,
                expires: expiresTime,
                signingCredentials: signingCredentials
            );
            // 生成 Token
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            //打包返回前台
            var responseJson = new TokenInfoResult
            {
                success = true,
                token = encodedJwt,
                expires = expiresTime,
                token_type = "Bearer"
            };
            return responseJson;
        }























        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueJwt(TokenModelJwt tokenModel)
        {
            string issuer = DMS.Common.AppConfig.GetValue(new string[] { "Audience", "Issuer" });
            string audience = DMS.Common.AppConfig.GetValue(new string[] { "Audience", "Audience" });
            string secretCredentials = DMS.Common.AppConfig.GetValue(new string[] { "Audience", "Secret" });

            //var claims = new Claim[] //old
            var claims = new List<Claim>
                {
                 /*
                 * 特别重要：
                   1、这里将用户的部分信息，比如 uid 存到了Claim 中，如果你想知道如何在其他地方将这个 uid从 Token 中取出来，请看下边的SerializeJwt() 方法，或者在整个解决方案，搜索这个方法，看哪里使用了！
                   2、你也可以研究下 HttpContext.User.Claims ，具体的你可以看看 Policys/PermissionHandler.cs 类中是如何使用的。
                 */

                    

                new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Uid.ToString()),
                new Claim(ClaimTypes.Name,tokenModel.Name),
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                //这个就是过期时间，目前是过期3600秒，可自定义，注意JWT有自己的缓冲过期时间
                new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(3600)).ToUnixTimeSeconds()}"),
                new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(1000).ToString()),
                new Claim(JwtRegisteredClaimNames.Iss,issuer),
                new Claim(JwtRegisteredClaimNames.Aud,audience),
                
                
                //new Claim(ClaimTypes.Role,tokenModel.Role),//为了解决一个用户多个角色(比如：Admin,System)，用下边的方法
               };

            // 可以将一个用户的多个角色全部赋予；
            claims.AddRange(tokenModel.Role.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));



            //秘钥 (SymmetricSecurityKey 对安全性的要求，密钥的长度太短会报出异常)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretCredentials));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                signingCredentials: creds
             );

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModelJwt SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            TokenModelJwt tokenModelJwt = new TokenModelJwt();

            // token校验
            if (jwtStr.IsNullOrEmpty() && jwtHandler.CanReadToken(jwtStr))
            {

                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);

                object role;

                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);

                tokenModelJwt = new TokenModelJwt
                {
                    Uid = (jwtToken.Id).ToInt(),
                    Role = role != null ? role.ToStringDefault() : "",
                };
            }
            return tokenModelJwt;
        }
        public static bool customSafeVerify(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var symmetricKeyAsBase64 = DMS.Common.AppConfig.GetValue(new string[] { "Audience", "Audience" });
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwt = jwtHandler.ReadJwtToken(token);
            return jwt.RawSignature == Microsoft.IdentityModel.JsonWebTokens.JwtTokenUtilities.CreateEncodedSignature(jwt.RawHeader + "." + jwt.RawPayload, signingCredentials);
        }
    }

    /// <summary>
    /// 令牌
    /// </summary>
    public class TokenModelJwt
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Uid { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }
    }
}

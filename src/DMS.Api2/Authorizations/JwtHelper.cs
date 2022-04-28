using DMS.Api2.Authorizations.Result;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DMS.Api2.Authorizations
{
    /// <summary>
    /// 
    /// </summary>
    public class JwtHelper
    {
        /// <summary>
        /// 创建TOKEN
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static TokenInfoResult Create(Claim[] claims)
        {
            #region 参数
            var thisTime = DateTime.Now;
            var option = AppConfig.JwtSettingOption;
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

    }
}

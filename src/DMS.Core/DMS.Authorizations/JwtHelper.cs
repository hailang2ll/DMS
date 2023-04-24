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
            //Permissions.ValidAudience = model.Uid + option.Audience + DateTime.Now.ToString();
            string issuer = option.Issuer;
            string audience = option.Audience;
            string secretCredentials = option.SecretKey;
            double expireMinutes = option.ExpireMinutes;

            var keyByteArray = Encoding.ASCII.GetBytes(secretCredentials);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            DateTime expiresTime = thisTime.AddMinutes(expireMinutes);
            #endregion


            string uuid = model.Uid + "^" + model.Cid + "^" + model.EpCode;
            string radmon = Guid.NewGuid().ToString().Replace("-", "");
            var claims = new List<Claim> {
                    new Claim(JwtClaimTypes.UuId, DMS.Common.Encrypt.DESHelper.Encode(uuid)),
                    new Claim(JwtClaimTypes.Radmon, radmon),
                    new Claim(JwtClaimTypes.Exp, expiresTime.ToTimestamp())
                };


            // 实例化JwtSecurityToken
            var jwt = new JwtSecurityToken(
                //issuer: issuer,
                //audience: audience,
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
        /// 解析token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static UserClaimModel ParseJwtToken(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            UserClaimModel userClaimModel = new UserClaimModel();

            // token校验
            //if (!token.IsNullOrEmpty() && jwtHandler.CanReadToken(token))
            //{
            //    JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(token);
            //    object uid = jwtToken.Payload.GetValueOrDefault(JwtClaimTypes.Uid);
            //    object cid = jwtToken.Payload.GetValueOrDefault(JwtClaimTypes.Cid);
            //    object epcode = jwtToken.Payload.GetValueOrDefault(JwtClaimTypes.EpCode);
            //    object role = jwtToken.Payload.GetValueOrDefault(JwtClaimTypes.Role);
            //    object expiration = jwtToken.Payload.GetValueOrDefault(JwtClaimTypes.Expiration);

            //    userClaimModel = new UserClaimModel
            //    {
            //        Uid = uid.ToLong(),
            //        Cid = cid.ToLong(),
            //        EpCode = epcode.ToStringDefault(),
            //        Role = role.ToStringDefault(),
            //        //Expiration = expiration.ToStringDefault(),
            //    };
            //}
            return userClaimModel;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool VerifyTokenSafe(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

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

            var jwt = jwtHandler.ReadJwtToken(token);
            string tokenSign = jwt.RawSignature;
            string tokenSign2 = Microsoft.IdentityModel.JsonWebTokens.JwtTokenUtilities.CreateEncodedSignature(jwt.RawHeader + "." + jwt.RawPayload, signingCredentials);
            return tokenSign == tokenSign2;
        }
    }

}

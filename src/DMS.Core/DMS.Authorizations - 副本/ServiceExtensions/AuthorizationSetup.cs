using DMS.Authorizations.Model;
using DMS.Authorizations.Policys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Authorizations.ServiceExtensions
{
    public static class AuthorizationSetup
    {
        /// <summary>
        /// 授权服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddAuthorizationSetup(this IServiceCollection services)
        {
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("BaseRole", options => options.RequireRole("admin_dylan"));
            //    options.AddPolicy("MoreBaseRole", options => options.RequireRole("admin_dylan", "user_dylan"));
            //});

            #region 参数
            JwtSettingModel option = DMS.Common.AppConfig.GetValue<JwtSettingModel>("JwtSetting");
            string issuer = option.Issuer;
            string audience = option.Audience;
            string secretCredentials = option.SecretKey;
            double expireMinutes = option.ExpireMinutes;

            var keyByteArray = Encoding.ASCII.GetBytes(secretCredentials);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // 如果要数据库动态绑定，这里先留个空，后边处理器里动态赋值
            var permission = new List<PermissionItem>(); ;

            // 角色与接口的权限要求参数
            // 如果第三个参数，是ClaimTypes.Role，上面集合的每个元素的Name为角色名称，如果ClaimTypes.Name，即上面集合的每个元素的Name为用户名
            var permissionRequirement = new PermissionRequirement(
                "/api/denied",
                permission,
                JwtClaimTypes.UniqueId,
                issuer,
                audience,
                signingCredentials,//签名凭据
                expiration: TimeSpan.FromMinutes(expireMinutes)//接口的过期时间
                );
            #endregion

            //基于自定义策略授权
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Permissions.Name,
                  policy => policy.Requirements.Add(permissionRequirement));
            });
            services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
            services.AddSingleton(permissionRequirement);
        }
    }
}

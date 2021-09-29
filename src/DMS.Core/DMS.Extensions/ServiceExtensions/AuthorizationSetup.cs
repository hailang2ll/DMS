#if NET5_0 || NETCOREAPP3_1
using DMS.Extensions.Authorizations.Policys;
using Microsoft.AspNetCore.Authorization;
#endif
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace DMS.Extensions.ServiceExtensions
{
    public static class AuthorizationSetup
    {
        /// <summary>
        /// 授权服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddAuthorizationSetup(this IServiceCollection services)
        {
#if NET5_0 || NETCOREAPP3_1
            services.AddAuthorization(options =>
            {
                options.AddPolicy("BaseRole", options => options.RequireRole("admin_dylan"));
                options.AddPolicy("MoreBaseRole", options => options.RequireRole("admin_dylan", "user_dylan"));
            });
            #region 参数
            string Issuer = DMSN.Common.CoreExtensions.AppConfig.GetVaule(new string[] { "Audience", "Issuer" });
            string Audience = DMSN.Common.CoreExtensions.AppConfig.GetVaule(new string[] { "Audience", "Audience" });
            string secretCredentials = DMSN.Common.CoreExtensions.AppConfig.GetVaule(new string[] { "Audience", "Secret" });

            var keyByteArray = Encoding.ASCII.GetBytes(secretCredentials);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // 如果要数据库动态绑定，这里先留个空，后边处理器里动态赋值
            var permission = new List<PermissionItem>();

            // 角色与接口的权限要求参数
            var permissionRequirement = new PermissionRequirement(
                "/api/denied",// 拒绝授权的跳转地址（目前无用）
                permission,
                ClaimTypes.Role,//基于角色的授权
                Issuer,//发行人
                Audience,//听众
                signingCredentials,//签名凭据
                expiration: TimeSpan.FromSeconds(60 * 60)//接口的过期时间
                );
            #endregion

            //基于自定义策略授权
            services.AddAuthorization(options =>
            {
                options.AddPolicy("customizePermisson",
                  policy => policy.Requirements.Add(permissionRequirement));
            });
            // 注入权限处理器
            services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
            //services.AddSingleton(permissionRequirement);
#endif
        }
    }
}

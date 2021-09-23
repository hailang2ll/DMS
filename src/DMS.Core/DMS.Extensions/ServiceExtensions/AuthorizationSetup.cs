#if NET5_0 || NETCOREAPP3_1
using DMS.Extensions.Authorizations.Policys;
using Microsoft.AspNetCore.Authorization;
#endif
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
                options.AddPolicy("BaseRole", options => options.RequireRole("admin"));
                options.AddPolicy("MoreBaseRole", options => options.RequireRole("admin", "user"));
            });

            //基于自定义策略授权
            services.AddAuthorization(options =>
            {
                options.AddPolicy("customizePermisson",
                  policy => policy
                    .Requirements
                    .Add(new PermissionRequirement("admin")));
            });
            //services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();
#endif
        }
    }
}

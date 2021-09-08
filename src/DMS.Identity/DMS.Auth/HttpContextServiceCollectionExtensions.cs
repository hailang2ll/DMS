using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DMS.Auth
{
    /// <summary>
    /// HttpContext 相关服务,迁移到DMS.Auth中 
    /// </summary>
    public static class HttpContextServiceCollectionExtensions
    {
        public static void AddHttpContextSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserAuth, UserAuth>();
        }
    }
}

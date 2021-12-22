using DMS.Extensions.Authorizations.Policys;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Extensions.ServiceExtensions
{
    public static class AuthenticationIds4Setup
    {
        public static void AddAuthenticationIds4Setup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));


            // 添加Identityserver4认证
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = nameof(ApiResponseHandler);
                o.DefaultForbidScheme = nameof(ApiResponseHandler);
            })
            .AddJwtBearer(options =>
            {
                options.Authority = DMS.Common.AppConfig.GetValue(new string[] { "IdentityServer4", "AuthorizationUrl" });
                options.RequireHttpsMetadata = false;
                options.Audience = DMS.Common.AppConfig.GetValue(new string[] { "IdentityServer4", "ApiName" });
            })
            .AddScheme<AuthenticationSchemeOptions, ApiResponseHandler>(nameof(ApiResponseHandler), o => { });
        }
    }
}

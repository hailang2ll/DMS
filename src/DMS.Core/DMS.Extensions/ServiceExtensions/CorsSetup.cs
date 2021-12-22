using DMS.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DMS.Extensions.ServiceExtensions
{
    public static class CorsSetup
    {
        public static void AddCorsSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            bool isEnableAllIPs = DMS.Common.AppConfig.GetValue(new string[] { "Cors", "EnableAllIPs" }).ToBool();
            var PolicyName = DMS.Common.AppConfig.GetValue(new string[] { "Cors", "PolicyName" });
            var IPs = DMS.Common.AppConfig.GetValue(new string[] { "Cors", "IPs" });
            DMS.Common.Helper.ConsoleHelper.WriteInfoLine($"AddCorsSetup:isEnableAllIPs={isEnableAllIPs},PolicyName={PolicyName},IPs={IPs}");
            services.AddCors(c =>
            {
                if (!isEnableAllIPs)
                {
                    c.AddPolicy(PolicyName,
                        policy =>
                        {

                            policy
                            .WithOrigins(IPs.Split(','))
                            .AllowAnyHeader()//Ensures that the policy allows any header.
                            .AllowAnyMethod();
                        });
                }
                else
                {
                    //允许任意跨域请求
                    c.AddPolicy(PolicyName,
                        policy =>
                        {
                            policy
                            .SetIsOriginAllowed((host) => true)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                        });
                }

            });
        }
    }
}

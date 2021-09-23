#if NET5_0 || NETCOREAPP3_1
using DMS.Extensions.Authorizations.Policys;
using Microsoft.AspNetCore.Authorization;
#endif
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DMS.Extensions.ServiceExtensions
{
    public static class AuthenticationJWTSetup
    {
        public static void AddAuthenticationJWTSetup(this IServiceCollection services)
        {

            string Issurer = DMSN.Common.CoreExtensions.AppConfig.GetVaule(new string[] { "Audience", "Issuer" });
            string Audience = DMSN.Common.CoreExtensions.AppConfig.GetVaule(new string[] { "Audience", "Audience" });
            string secretCredentials = DMSN.Common.CoreExtensions.AppConfig.GetVaule(new string[] { "Audience", "Secret" });

            //认证服务
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o => {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    //是否验证发行人
                    ValidateIssuer = true,
                    ValidIssuer = Issurer,//发行人
                    //是否验证受众人
                    ValidateAudience = true,
                    ValidAudience = Audience,//受众人
                    //是否验证密钥
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretCredentials)),

                    ValidateLifetime = true, //验证生命周期
                    RequireExpirationTime = true, //过期时间
                };
            });

#if NET5_0 || NETCOREAPP3_1
           
#endif
        }
    }
}

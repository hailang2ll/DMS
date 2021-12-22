#if NET5_0 || NETCOREAPP3_1
using DMS.Extensions.Authorizations.Policys;
using Microsoft.AspNetCore.Authorization;
#endif
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using DMS.Common.Extensions;

namespace DMS.Extensions.ServiceExtensions
{
    public static class AuthenticationJWTSetup
    {
        public static void AddAuthenticationJWTSetup(this IServiceCollection services)
        {

            string Issuer = DMS.Common.AppConfig.GetValue(new string[] { "Audience", "Issuer" });
            string Audience = DMS.Common.AppConfig.GetValue(new string[] { "Audience", "Audience" });
            string secretCredentials = DMS.Common.AppConfig.GetValue(new string[] { "Audience", "Secret" });

            // 令牌验证参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                //是否验证发行人
                ValidateIssuer = true,
                ValidIssuer = Issuer,//发行人
                                     //是否验证受众人
                ValidateAudience = true,
                ValidAudience = Audience,//受众人
                                         //是否验证密钥
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretCredentials)),

                ValidateLifetime = true, //验证生命周期
                //ClockSkew = TimeSpan.FromSeconds(30),
                RequireExpirationTime = true, //过期时间
            };
#if NET5_0 || NETCOREAPP3_1
            //开启Bearer认证
            services.AddAuthentication(x =>
            {
                //x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = nameof(ApiResponseHandler);
                x.DefaultForbidScheme = nameof(ApiResponseHandler);
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = tokenValidationParameters;
                o.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.Response.Headers.Add("Token-Error", context.ErrorDescription);
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var jwtHandler = new JwtSecurityTokenHandler();
                        var token = context.Request.Headers["Authorization"].ToStringDefault().Replace("Bearer ", "");

                        if (!token.IsNullOrEmpty() && jwtHandler.CanReadToken(token))
                        {
                            var jwtToken = jwtHandler.ReadJwtToken(token);

                            if (jwtToken.Issuer != Issuer)
                            {
                                context.Response.Headers.Add("Token-Error-Iss", "issuer is wrong!");
                            }

                            if (jwtToken.Audiences.FirstOrDefault() != Audience)
                            {
                                context.Response.Headers.Add("Token-Error-Aud", "Audience is wrong!");
                            }
                        }


                        // 如果过期，则把<是否过期>添加到，返回头信息中
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            })
            .AddScheme<AuthenticationSchemeOptions, ApiResponseHandler>(nameof(ApiResponseHandler), o => { });



#endif
        }
    }
}

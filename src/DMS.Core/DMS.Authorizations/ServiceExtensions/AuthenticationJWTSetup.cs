using DMS.Authorizations.Model;
using DMS.Authorizations.Policys;
using DMS.Common.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Authorizations.ServiceExtensions
{
    public static class AuthenticationJWTSetup
    {
        public static void AddAuthenticationJWTSetup(this IServiceCollection services)
        {
            JwtSettingModel option = DMS.Common.AppConfig.GetValue<JwtSettingModel>("JwtSetting");
            string issuer = option.Issuer;
            string audience = option.Audience;
            string secretCredentials = option.SecretKey;

            // 令牌验证参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,//是否验证发行人
                ValidIssuer = issuer,//发行人

                ValidateAudience = false,//是否验证被发布者
                ValidAudience = audience,//受众人
                //这里采用动态验证的方式，在重新登陆时，刷新token，旧token就强制失效了
                //AudienceValidator = (m, n, z) =>
                //{
                //    return m != null && m.FirstOrDefault().Equals(Permissions.ValidAudience);
                //},

                ValidateIssuerSigningKey = true,//是否验证密钥
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretCredentials)),

                ValidateLifetime = true, //验证生命周期
                ClockSkew = TimeSpan.FromSeconds(30),//注意这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
                RequireExpirationTime = true, //过期时间
            };
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
                //不使用https
                //o.RequireHttpsMetadata = false;
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
                        //var jwtHandler = new JwtSecurityTokenHandler();
                        //var token = context.Request.Headers["Authorization"].ToStringDefault().Replace("Bearer ", "");

                        //if (!token.IsNullOrEmpty() && jwtHandler.CanReadToken(token))
                        //{
                        //    var jwtToken = jwtHandler.ReadJwtToken(token);

                        //    if (jwtToken.Issuer != issuer)
                        //    {
                        //        context.Response.Headers.Add("Token-Error-Iss", "issuer is wrong!");
                        //    }

                        //    if (jwtToken.Audiences.FirstOrDefault() != audience)
                        //    {
                        //        context.Response.Headers.Add("Token-Error-Aud", "Audience is wrong!");
                        //    }
                        //}

                        string headKey = "";
                        // 如果过期，则把<是否过期>添加到，返回头信息中
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        else if (context.Exception.GetType() == typeof(SecurityTokenNoExpirationException))
                        {
                            //IDX10225: Lifetime validation failed. The token is missing an Expiration Time. Tokentype: 'System.String'.
                            var message = context.Exception.Message;
                            if (message.Contains("IDX10225:"))
                            {
                                headKey = "The-token-is-missing-an-Expiration-Time";
                            }
                            context.Response.Headers.Add(headKey, "true");
                        }
                        else if (context.Exception.GetType() == typeof(ArgumentException))
                        {
                            var message = context.Exception.Message;
                            if (message.Contains("IDX12729:"))
                            {
                                headKey = "Header-Alter";
                            }
                            else if (message.Contains("IDX12723:"))
                            {
                                headKey = "Payload-Alter";
                            }
                            else if (message.Contains("IDX10503:"))
                            {
                                headKey = "Signature-Alter";
                            }
                            else
                            {
                                headKey = "None-Alter";
                            }
                            context.Response.Headers.Add(headKey, "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            })
            .AddScheme<AuthenticationSchemeOptions, ApiResponseHandler>(nameof(ApiResponseHandler), o => { });



        }
    }
}

using DMS.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DMS.Auth
{
    public static class AuthSetup
    {
        public static void AddAuthSetup(this IServiceCollection services, AuthModel authModel = AuthModel.Token)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (authModel == AuthModel.Token)
            {
                services.AddScoped<DMS.Auth.Token.IUserAuth, DMS.Auth.Token.UserAuth>();
                DMS.Common.Helper.ConsoleHelper.WriteSuccessLine($"AddAuthSetup:我是普通Token认证方式");
            }
            else if (authModel == AuthModel.Auth20)
            {
                services.AddScoped<DMS.Auth.Oauth2.IUserAuth, DMS.Auth.Oauth2.UserAuth>();
                DMS.Common.Helper.ConsoleHelper.WriteSuccessLine($"AddAuthSetup:我是Oauth2认证方式");
            }
            else
            {
                services.AddScoped<DMS.Auth.Token.IUserAuth, DMS.Auth.Token.UserAuth>();
                DMS.Common.Helper.ConsoleHelper.WriteSuccessLine($"AddAuthSetup:我是普通Token认证方式");
                services.AddScoped<DMS.Auth.Oauth2.IUserAuth, DMS.Auth.Oauth2.UserAuth>();
                DMS.Common.Helper.ConsoleHelper.WriteSuccessLine($"AddAuthSetup:我是Oauth2认证方式");
            }
        }
    }
}

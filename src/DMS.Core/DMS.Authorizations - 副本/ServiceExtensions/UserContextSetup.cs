using DMS.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DMS.Authorizations.ServiceExtensions
{
    public static class UserContextSetup
    {
        public static void AddUserContextSetup(this IServiceCollection services, AuthModel authModel = AuthModel.Jwt)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (authModel == AuthModel.Token)
            {
                services.AddScoped<DMS.Authorizations.UserContext.Token.IUserAuth, DMS.Authorizations.UserContext.Token.UserAuth>();
                DMS.Common.Helper.ConsoleHelper.WriteSuccessLine($"AddAuthSetup:我是普通Token认证方式");
            }
            else if (authModel == AuthModel.Jwt)
            {
                services.AddScoped<DMS.Authorizations.UserContext.Jwt.IUserAuth, DMS.Authorizations.UserContext.Jwt.UserAuth>();
                DMS.Common.Helper.ConsoleHelper.WriteSuccessLine($"AddAuthSetup:我是Oauth2认证方式");
            }
            else
            {
                services.AddScoped<DMS.Authorizations.UserContext.Token.IUserAuth, DMS.Authorizations.UserContext.Token.UserAuth>();
                DMS.Common.Helper.ConsoleHelper.WriteSuccessLine($"AddAuthSetup:我是普通Token认证方式");
                services.AddScoped<DMS.Authorizations.UserContext.Jwt.IUserAuth, DMS.Authorizations.UserContext.Jwt.UserAuth>();
                DMS.Common.Helper.ConsoleHelper.WriteSuccessLine($"AddAuthSetup:我是Jwt认证方式");
            }
        }
    }
}

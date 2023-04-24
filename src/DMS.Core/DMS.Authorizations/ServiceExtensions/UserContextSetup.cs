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
            if (authModel == AuthModel.Cookies)
            {
                services.AddScoped<DMS.Authorizations.UserContext.Cookies.IUserAuth, DMS.Authorizations.UserContext.Cookies.UserAuth>();
                DMS.Common.Helper.ConsoleHelper.WriteSuccessLine($"AddAuthSetup:我是cookies认证方式注入");
            }
            else if (authModel == AuthModel.Token)
            {
                services.AddScoped<DMS.Authorizations.UserContext.Token.IUserAuth, DMS.Authorizations.UserContext.Token.UserAuth>();
                DMS.Common.Helper.ConsoleHelper.WriteSuccessLine($"AddAuthSetup:我是token认证方式注入");
            }
            else if (authModel == AuthModel.Jwt)
            {
                services.AddScoped<DMS.Authorizations.UserContext.Jwt.IUserAuth, DMS.Authorizations.UserContext.Jwt.UserAuth>();
                DMS.Common.Helper.ConsoleHelper.WriteSuccessLine($"AddAuthSetup:我是jwt+redis认证方式注入");
            }
            else if (authModel == AuthModel.Id4)
            {
                DMS.Common.Helper.ConsoleHelper.WriteSuccessLine($"AddAuthSetup:我是id4+redis认证方式注入，还未实现");
            }
            else
            {
                DMS.Common.Helper.ConsoleHelper.WriteSuccessLine($"AddAuthSetup:未知认证注入");
            }
        }
    }
}

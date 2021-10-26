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
                services.AddScoped<DMS.Auth.v1.IUserAuth, DMS.Auth.v1.UserAuth>();
            }
            else if (authModel == AuthModel.Auth20)
            {
                services.AddScoped<DMS.Auth.v2.IUserAuth, DMS.Auth.v2.UserAuth>();
            }
        }
    }
}

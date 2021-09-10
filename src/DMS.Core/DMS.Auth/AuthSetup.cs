using Microsoft.Extensions.DependencyInjection;
using System;

namespace DMS.Auth
{
    public static class AuthSetup
    {
        public static void AddAuthSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<IUserAuth, UserAuth>();
        }
    }
}

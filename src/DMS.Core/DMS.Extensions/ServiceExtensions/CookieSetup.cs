using DMS.Extensions.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DMS.Extensions.ServiceExtensions
{
    public static class CookieSetup
    {
        public static void AddCookieSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<ICookieHelper, CookieHelper>();
        }
    }
}

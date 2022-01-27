using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Extensions.ServiceExtensions
{
    public static class AuthenticationCookiesSetup
    {
        public static void AddAuthenticationCookiesSetup(this IServiceCollection services)
        {

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                   .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                   {
                       o.Cookie.Name = "_AdminTicketCookie";
                       o.LoginPath = new PathString("/Account/Login");
                       o.LogoutPath = new PathString("/Account/Login");
                       o.AccessDeniedPath = new PathString("/Error/Forbidden");
                   });


        }
    }
}

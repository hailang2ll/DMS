#if !net461
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DMS.Common.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    public static class RegisterHttpContextExtensions
    {
        private static IHttpContextAccessor _httpContext;
        public static Microsoft.AspNetCore.Http.HttpContext Current => _httpContext.HttpContext;
        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            _httpContext = httpContextAccessor;
            return app;
        }
    }
}

#endif
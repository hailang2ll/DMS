using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DMS.Swagger
{
    public static class SwaggerUIBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerUIV2(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", AppDomain.CurrentDomain.FriendlyName);
                c.DocExpansion(DocExpansion.None);
            });
            Console.WriteLine($"SwaggerUI加载完成");
            return app;
        }
    }
}

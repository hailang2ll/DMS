using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DMS.Swagger
{
    public static class SwaggerUIBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="isDebug"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerUIV2(this IApplicationBuilder app, bool isDebug)
        {
            if (isDebug)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", AppDomain.CurrentDomain.FriendlyName);
                    //c.DocExpansion(DocExpansion.None);
                });
            }
            return app;
        }
    }
}

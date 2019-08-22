using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace DMS.Swagger
{
    public static class SwaggerGenServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGenV2(this IServiceCollection services)
        {
            var basePath = AppContext.BaseDirectory;
            var commentsFileName = AppDomain.CurrentDomain.FriendlyName + ".xml";
            var xmlPath = Path.Combine(basePath, commentsFileName);
            if (File.Exists(xmlPath))
            {
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new Info
                    {
                        Version = "Version 1.0",
                        Title = AppDomain.CurrentDomain.FriendlyName,
                        Description = "框架说明文档",
                        TermsOfService = "None",
                        Contact = new Swashbuckle.AspNetCore.Swagger.Contact { Name = "Learn.Swagger", Email = "79522860@qq.com", Url = "https://www.trydou.com" }

                    });

                    options.IncludeXmlComments(xmlPath);

                    options.OperationFilter<AssignOperationVendorExtensions>();
                });
            }

            return services;
        }


        /// <summary>
        /// 
        /// </summary>
        public class AssignOperationVendorExtensions : IOperationFilter
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="operation"></param>
            /// <param name="context"></param>
            public void Apply(Operation operation, OperationFilterContext context)
            {
                operation.Parameters = operation.Parameters ?? new List<IParameter>();
                operation.Parameters.Add(new NonBodyParameter()
                {
                    In = "header",
                    Type = "string",
                    Required = false,
                    Name = "AccessToken",
                    Description = "访问令牌"
                });
            }
        }
    }
}

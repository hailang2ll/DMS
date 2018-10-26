using DMS.BaseFramework.Common.DI;
using DMS.Exceptionless.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DMS.CommonWebApiTest
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            #region 日志文件加载一：在此配置也是可以；加载二：在Program加载也是可以的
            //var currentDir = Directory.GetCurrentDirectory();
            //var log4netPath = $@"{currentDir}\Config\log4net.config";
            //configuration.ConfigureLog4net(log4netPath);//指定log4net配置文件

            //var nlogPath = $@"{currentDir}\Config\nlog.config";
            //configuration.ConfigureNLog(nlogPath);
            #endregion

        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });

            #region 注入HttpContext
            //services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
            services.AddHttpContextAccessor();

            #region Swagger
#if DEBUG
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Version = "Version 1.0",
                    Description = "Dylan作者",
                    Title = "DMS的API文档"
                });

                var basePath = AppContext.BaseDirectory;
                var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".XML";
                options.IncludeXmlComments(Path.Combine(basePath, commentsFileName));
                options.OperationFilter<AssignOperationVendorExtensions>();
            });
#endif
            #endregion
            #endregion

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="lifetime"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DMS.CommonWebApiTest");
                    c.DocExpansion(DocExpansion.None);
                });
            }
            else
            {
                app.UseHsts();
            }
            app.UseStaticFiles();

            #region 使用HttpContext
            app.UseStaticHttpContext();
            #endregion

            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            #region register this service
            //ConsulService consulService = new ConsulService()
            //{
            //    IP = Configuration["Consul:IP"],
            //    Port = Convert.ToInt32(Configuration["Consul:Port"])
            //};
            //HealthService healthService = new HealthService()
            //{
            //    IP = Configuration["Service:IP"],
            //    Port = Convert.ToInt32(Configuration["Service:Port"]),
            //    Name = Configuration["Service:Name"],
            //};
            //app.RegisterConsul(lifetime, healthService, consulService);
            #endregion
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

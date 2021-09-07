using Autofac;
using DMS.Autofac;
using DMS.NLogs.Filters;
using DMS.Redis.Configurations;
using DMS.Swagger;
using DMSN.Common.Configurations;
using DMSN.Common.Helper;
using DMSN.Common.JsonHandler.JsonConverters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Sample31.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public Startup(IWebHostEnvironment env)
        {
            var path = env.ContentRootPath;
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddRedisFile($"Configs/redis.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"Configs/domain.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
            .AddAppSettingsFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }

    

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(option =>
            {
                option.Filters.Add<GlobalExceptionFilter>();
                //option.Filters.Add(typeof(LoginFilter));

            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.DictionaryKeyPolicy = null;
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSwaggerGenV2();
            services.AddRedisSetup();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUIV2(DebugHelper.IsDebug(GetType()));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAutofac31();
        }
    }
}

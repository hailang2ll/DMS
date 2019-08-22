using DMS.Common.Configurations;
using DMS.Log4net;
using DMS.Redis;
using DMS.Redis.Configurations;
using DMS.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace DMS.Sample
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var path = env.ContentRootPath;
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddRedisFile($"Configs/redis.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"Configs/domain.json", optional: false, reloadOnChange: true)
            .AddAppSettingsFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();

            #region redis 订阅
            //RedisManager redisManager = new RedisManager(2);
            //redisManager.Subscribe("messages", (channel, message) => {

            //    //输出收到的消息
            //    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
            //});
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSwaggerGenV2();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUIV2();
            }

            app.UseStaticHttpContext();
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


    }
}

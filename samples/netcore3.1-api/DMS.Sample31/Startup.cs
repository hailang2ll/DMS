using Autofac;
using DMS.Autofac;
using DMS.Common.Configurations;
using DMS.Common.Serialization.JsonConverters;
using DMS.Log4net.Filters;
using DMS.Redis.Configurations;
using DMS.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DMS.Sample31
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
            }
            app.UseSwaggerUIV2();
            app.UseStaticHttpContext();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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
        /// <param name="containerBuilder"></param>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAutofac31();
        }


    }

    
}

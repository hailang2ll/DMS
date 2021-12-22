using Autofac;
using DMS.Auth;
using DMS.Autofac;
using DMS.Common.Extensions;
using DMS.Common.Helper;
using DMS.Common.JsonHandler.JsonConverters;
using DMS.Extensions;
using DMS.Extensions.Authorizations.Model;
using DMS.Extensions.ServiceExtensions;
using DMS.NLogs.Filters;
using DMS.Redis.Configurations;
using DMS.Sample.Service.RedisEvBus;
using DMS.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DMS.Sample.Api
{
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
            .AddJsonFile($"Configs/redis.json", optional: false, reloadOnChange: true)
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
            services.AddSwaggerGenV2(AuthModel.Token);
            services.AddHttpContextSetup();
            services.AddRedisSetup();
            services.AddAuthSetup(AuthModel.Token);
            //用户服务自定义
            services.AddRedisMqSetup();
            services.AddCorsSetup();


            // 授权+认证 (jwt or ids4)
            if (Permissions.IsUseIds4)
            {
                services.AddAuthenticationIds4Setup();
            }
            else
            {
                services.AddAuthenticationJWTSetup();
            }
            //services.AddAuthorizationSetup();

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
            app.UseSwaggerUIV2(DebugHelper.IsDebug(GetType()));
            // CORS跨域
            app.UseCors(DMS.Common.AppConfig.GetValue(new string[] { "Cors", "PolicyName" }));
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
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

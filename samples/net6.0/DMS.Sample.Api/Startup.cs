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
                //ȫ�ִ����쳣��֧��DMS.Log4net��DMS.NLogs
                option.Filters.Add<GlobalExceptionFilter>();
                //option.Filters.Add(typeof(LoginFilter));

            }).AddJsonOptions(options =>
            {   
                options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.DictionaryKeyPolicy = null;
            });
            //api�ĵ����ɣ�1֧����ͨtoken��֤��2֧��oauth2�л���Ĭ��Ϊ1
            //services.AddSwaggerGenV2();
            ////����HttpContext����
            //services.AddHttpContextSetup();
            ////����redis����
            //services.AddRedisSetup();
            ////����������֤������api�ĵ���֤��Ӧ
            //services.AddAuthSetup();
            ////����redismq����
            //services.AddRedisMqSetup();
            ////�����������
            //services.AddCorsSetup();


            //// ��Ȩ+��֤ (jwt or ids4)
            //if (Permissions.IsUseIds4)
            //{
            //    services.AddAuthenticationIds4Setup();
            //}
            //else
            //{
            //    services.AddAuthenticationJWTSetup();
            //}


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
            //app.UseSwaggerUIV2(DebugHelper.IsDebug(GetType()));
            // CORS����
            app.UseCors(DMS.Common.AppConfig.GetValue(new string[] { "Cors", "PolicyName" }));
            //������̬ҳ��
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// �ӿ�ע��
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAutofac31();
        }

    }
}
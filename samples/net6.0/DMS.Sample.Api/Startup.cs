using Autofac;
using DMS.Auth;
using DMS.Common.Extensions;
using DMS.Common.Helper;
using DMS.Common.JsonHandler.JsonConverters;
using DMS.Common.Model.Result;
using DMS.Extensions;
using DMS.Extensions.Authorizations.Model;
using DMS.Extensions.ServiceExtensions;
using DMS.NLogs.Filters;
using DMS.Redis.Configurations;
using DMS.Sample.Api.Filter;
using DMS.Sample.Service.RedisEvBus;
using DMS.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

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
                //全局处理异常，支持DMS.Log4net，DMS.NLogs
                option.Filters.Add<GlobalExceptionFilter>();

            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));
                //options.JsonSerializerOptions.PropertyNamingPolicy = null;
                //options.JsonSerializerOptions.DictionaryKeyPolicy = null;
            }).ConfigureApiBehaviorOptions(options =>
            {
                //使用自定义模型验证
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var result = new ResponseResult()
                    {
                        errno = 1,
                        errmsg = string.Join(Environment.NewLine, context.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))
                    };
                    return new JsonResult(result);
                };
            });
            #region 模型验证三种方法，选择一种即可
            //第一种：模型全局验证
            //option.Filters.Add<ApiResultFilterAttribute>();
            //.ConfigureApiBehaviorOptions(options =>
            // {
            //     options.SuppressModelStateInvalidFilter = true;//关闭验证，param json转换为空不报错
            // });


            //第二种：模型全局验证
            //.ConfigureApiBehaviorOptions(options =>
            // {
            //     //使用自定义模型验证
            //     options.InvalidModelStateResponseFactory = (context) =>
            //     {
            //         var result = new ResponseResult();
            //         result.errmsg = string.Join(Environment.NewLine, context.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
            //         return new JsonResult(result);
            //     };
            // });


            //第三种：模型全局验证
            //services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.InvalidModelStateResponseFactory = (context) =>
            //    {
            //        var result = new ResponseResult();
            //        result.errmsg = string.Join(Environment.NewLine, context.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
            //        return new JsonResult(result);
            //    };
            //});
            #endregion


            //api文档生成，1支持普通token验证，2支持oauth2切换；默认为1
            services.AddSwaggerGenSetup(option =>
            {
                option.RootPath = AppContext.BaseDirectory;
                option.XmlFiles = new List<string> {
                     AppDomain.CurrentDomain.FriendlyName+".xml",
                     "DMS.Sample.Contracts.xml"
                };
            });
            services.AddSqlsugarSetup();
            //开启redis服务
            services.AddRedisSetup();
            //开启redismq服务
            services.AddRedisMqSetup();
            //开启HttpContext服务
            services.AddHttpContextSetup();
            //开启身份认证服务，与api文档验证对应即可
            services.AddAuthSetup();


            Permissions.IsUseIds4 = DMS.Common.AppConfig.GetValue(new string[] { "IdentityServer4", "Enabled" }).ToBool();
            services.AddAuthorizationSetup();
            // 授权+认证 (jwt or ids4)
            if (Permissions.IsUseIds4)
            {
                services.AddAuthenticationIds4Setup();
            }
            else
            {
                services.AddAuthenticationJWTSetup();
            }

            ////开启跨域服务
            //services.AddCorsSetup();
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

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
            app.UseSwaggerUI(DebugHelper.IsDebug(GetType()));
            // CORS跨域
            app.UseCors(DMS.Common.AppConfig.GetValue(new string[] { "Cors", "PolicyName" }));
            //开户静态页面
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
        /// 接口注入
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister(AppContext.BaseDirectory, new List<string>()
            {
                "DMS.Sample.Service.dll",
            }));
            builder.RegisterModule<AutofacPropertityModuleRegister>();
        }

    }
}

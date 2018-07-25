using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.BaseFramework.Common.DI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DMS.WebAPITest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            services.AddHttpContextAccessor();

            //options.Filters.Add(typeof(ActionFilter));

            // services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();


            //#region 系统自带容器，开启后，以下代码代码都不需要
            ////services.AddTransient<IMemberAddressService, MemberAddressService>();
            //#endregion

            //#region DI注入几种方式
            //var builder = new ContainerBuilder();

            //#region 单个注入
            ////builder.RegisterType<MemberAddressService>().As<IMemberAddressService>();
            //#endregion

            //var assemblys = Assembly.Load("CSTJR.Member.Service");
            //builder.RegisterAssemblyTypes(assemblys)
            //    .Where(t => t.Name.EndsWith("Service"))
            //    .AsImplementedInterfaces().InstancePerLifetimeScope();

            //#region 通过一个统一的接口来注入
            ////var assemblys = Assembly.Load("CSTJR.Member.Service");
            ////var baseType = typeof(IDependency);//IDependency 是一个接口  
            ////builder.RegisterAssemblyTypes(assemblys)
            //// .Where(m => baseType.IsAssignableFrom(m) && m != baseType)
            //// .AsImplementedInterfaces().InstancePerLifetimeScope();
            //#endregion
            ////containerBuilder.RegisterModule<DefaultModule>(); 需要测试 

            //builder.Populate(services);
            //this.ApplicationContainer = builder.Build();
            //return new AutofacServiceProvider(this.ApplicationContainer);//第三方IOC接管 core内置DI容器
            //#endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //
            app.UseStaticHttpContext();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

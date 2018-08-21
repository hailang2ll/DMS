using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DMS.Autofac
{
    public class AutofacService
    {
        #region Autofac注册

        /// <summary>
        /// Autofac属性注入
        /// 实现接口项目以.Service结尾，并继承全局接口IAutofacBase即可
        /// 继承后不需配置任何数据，即可用属性来调用方法
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceProvider RegisterAutofacByInherit(IServiceCollection services,string serviceName)
        {

            #region Autofac DI注入 第四种

            #region 如果在活动程序集中用此方法
            //Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(q => q.FullName.Contains("Trunk")).ToArray();
            //var types = Assembly.GetEntryAssembly().GetReferencedAssemblies();
            #endregion


            //前程序域名称
            //var currentDomainName = AppDomain.CurrentDomain.FriendlyName;
            //查找以Service结尾的程序集，不在活动程序集中用些方法
            //String baseDir = AppDomain.CurrentDomain.BaseDirectory; //AppContext.BaseDirectory; //String basePath2 = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            //DirectoryInfo dirInfo = new DirectoryInfo(baseDir);
            //var serviceFileInfo = dirInfo.GetFiles().Where(q => q.FullName.Contains(currentDomainName + ".Service")).FirstOrDefault();
            

            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            //var assembly = this.GetType().GetTypeInfo().Assembly;//如果在Startup中代码可以替换这句
            var assembly = Assembly.GetEntryAssembly(); //AppDomain.CurrentDomain.GetAssemblies().Where(q => q.FullName.Contains(currentDomainName)).FirstOrDefault();
            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(assembly));
            manager.FeatureProviders.Add(new ControllerFeatureProvider());
            var feature = new ControllerFeature();
            manager.PopulateFeature(feature);


            var builder = new ContainerBuilder();
            builder.RegisterType<ApplicationPartManager>().AsSelf().SingleInstance();
            builder.RegisterTypes(feature.Controllers.Select(ti => ti.AsType()).ToArray()).PropertiesAutowired();
            builder.Populate(services);

            var assemblys = Assembly.Load(serviceName);
            var baseType = typeof(IAutofacBase);
            builder.RegisterAssemblyTypes(assemblys)
             .Where(m => baseType.IsAssignableFrom(m) && m != baseType)
             .AsImplementedInterfaces().InstancePerLifetimeScope();

            return new AutofacServiceProvider(builder.Build());
            #endregion
        }

        /// <summary>
        /// Autofac属性注入
        /// 接口项目以.Contracts结尾，实现接口项目以.Service结尾
        /// 调用此方法不需要继承也不需要配置，即可属性注入调用
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceProvider RegisterAutofac(IServiceCollection services, string iserviceName, string serviceName)
        {

            #region Autofac DI注入 第三种
            //前程序域名称
            //var currentDomainName = AppDomain.CurrentDomain.FriendlyName;
            //查找以Service结尾的程序集，不在活动程序集中用些方法
            //String baseDir = AppDomain.CurrentDomain.BaseDirectory; //AppContext.BaseDirectory; //String basePath2 = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            //DirectoryInfo dirInfo = new DirectoryInfo(baseDir);
            //var serviceFileInfo = dirInfo.GetFiles().Where(q => q.FullName.Contains(currentDomainName + ".Service")).FirstOrDefault();
            //var contractsFileInfo = dirInfo.GetFiles().Where(q => q.FullName.Contains(currentDomainName + ".Contracts")).FirstOrDefault();


            //说明：ConfigureServices返回IServiceProvider，通过接口属性来获取,自动获取Controller的属性
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            //var assembly = this.GetType().GetTypeInfo().Assembly;
            var assembly = Assembly.GetEntryAssembly(); //AppDomain.CurrentDomain.GetAssemblies().Where(q => q.FullName.Contains(currentDomainName)).FirstOrDefault();
            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(assembly));
            manager.FeatureProviders.Add(new ControllerFeatureProvider());
            var feature = new ControllerFeature();
            manager.PopulateFeature(feature);


            var builder = new ContainerBuilder();
            builder.RegisterType<ApplicationPartManager>().AsSelf().SingleInstance();
            builder.RegisterTypes(feature.Controllers.Select(ti => ti.AsType()).ToArray()).PropertiesAutowired();
            builder.Populate(services);

            Assembly iservice = Assembly.Load(iserviceName);
            Assembly service = Assembly.Load(serviceName);
            builder.RegisterAssemblyTypes(iservice, service)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            return new AutofacServiceProvider(builder.Build());
            #endregion
        }

        #endregion
    }
}

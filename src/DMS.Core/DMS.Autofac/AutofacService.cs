using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace DMS.Autofac
{
    public class AutofacService
    {
        /// <summary>
        /// Autofac属性注入
        /// 接口项目以.Contracts结尾，实现接口项目以.Service结尾
        /// 调用此方法不需要继承也不需要配置，即可属性注入调用
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceProvider RegisterAutofac(IServiceCollection services, EnumAutoInject autoInject = EnumAutoInject.指定接口与实现程序集)
        {
            //说明：ConfigureServices返回IServiceProvider，通过接口属性来获取,自动获取Controller的属性
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            var assembly = Assembly.GetEntryAssembly();
            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(assembly));
            manager.FeatureProviders.Add(new ControllerFeatureProvider());
            var feature = new ControllerFeature();
            manager.PopulateFeature(feature);

            var builder = new ContainerBuilder();
            builder.RegisterType<ApplicationPartManager>().AsSelf().SingleInstance();
            builder.RegisterTypes(feature.Controllers.Select(ti => ti.AsType()).ToArray()).PropertiesAutowired();
            builder.Populate(services);//将services中的服务填充到Autofac中

            if (autoInject == EnumAutoInject.指定接口与实现程序集)
            {
                #region 指定接口与实现程序集
                string serviceName = AppDomain.CurrentDomain.FriendlyName.Replace(".Api", "").Replace("Api", "").Replace(".API", "").Replace("API", "");
                Console.WriteLine($"RegisterAutofac:serviceName={serviceName}");
                string iserviceName = serviceName;
                if (serviceName.EndsWith('.'))
                {
                    iserviceName += "Contracts";
                    serviceName += "Service";
                }
                else
                {
                    iserviceName += ".Contracts";
                    serviceName += ".Service";
                }
                Assembly iservice = Assembly.Load(iserviceName);
                Assembly service = Assembly.Load(serviceName);
                builder.RegisterAssemblyTypes(iservice, service)
                    .Where(t => t.Name.EndsWith("Service"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
                return new AutofacServiceProvider(builder.Build());
                #endregion
            }
            else if (autoInject == EnumAutoInject.指定IAutoInject与实现程序集)
            {
                #region 指定IAutoInject与实现程序集
                string serviceName = AppDomain.CurrentDomain.FriendlyName.Replace(".Api", "").Replace("Api", "").Replace(".API", "").Replace("API", "");
                Console.WriteLine($"RegisterAutofac:serviceName={serviceName}");
                if (serviceName.EndsWith('.'))
                {
                    serviceName += "Service";
                }
                else
                {
                    serviceName += ".Service";
                }

                var assemblys = Assembly.Load(serviceName);//Service是继承接口的实现方法类库名称
                var baseType = typeof(IAutoInject);//IDependency 是一个接口（所有要实现依赖注入的接口都要继承该接口）
                builder.RegisterAssemblyTypes(assemblys)
                 .Where(m => baseType.IsAssignableFrom(m) && m != baseType)
                 .AsImplementedInterfaces()
                 .InstancePerLifetimeScope();
                return new AutofacServiceProvider(builder.Build());
                #endregion
            }
            else
            {
                #region 自动加载接口与实现
                var assemblys = AppDomain.CurrentDomain.GetAssemblies().ToArray();
                //var assemblys = AppDomain.CurrentDomain.GetAssemblies().Where(q => q.FullName.Contains(".Service")).FirstOrDefault();
                builder.RegisterAssemblyTypes(assemblys)
                    .Where(t => t.Name.EndsWith("Service"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
                return new AutofacServiceProvider(builder.Build());
                #endregion
            }
        }

        /// <summary>
        /// 指定类注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="iserviceName"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static IServiceProvider RegisterAutofac(IServiceCollection services, string iserviceName, string serviceName)
        {
            //说明：ConfigureServices返回IServiceProvider，通过接口属性来获取,自动获取Controller的属性
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            var assembly = Assembly.GetEntryAssembly();
            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(assembly));
            manager.FeatureProviders.Add(new ControllerFeatureProvider());
            var feature = new ControllerFeature();
            manager.PopulateFeature(feature);

            var builder = new ContainerBuilder();
            builder.RegisterType<ApplicationPartManager>().AsSelf().SingleInstance();
            builder.RegisterTypes(feature.Controllers.Select(ti => ti.AsType()).ToArray()).PropertiesAutowired();
            builder.Populate(services);//将services中的服务填充到Autofac中


            Assembly iservice = Assembly.Load(iserviceName);
            Assembly service = Assembly.Load(serviceName);
            builder.RegisterAssemblyTypes(iservice, service)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            return new AutofacServiceProvider(builder.Build());
        }
    }
}

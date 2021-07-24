using Autofac;
using System;
using System.Reflection;

namespace DMS.Autofac
{
    public static class AutofacService31
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="serviceName"></param>
        /// <param name="iserviceName"></param>
        public static void RegisterAutofac31(this ContainerBuilder containerBuilder, string serviceName = "", string iserviceName = "")
        {
            if (string.IsNullOrEmpty(serviceName) || string.IsNullOrEmpty(iserviceName))
            {
                serviceName = AppDomain.CurrentDomain.FriendlyName.Replace(".Api", "").Replace("Api", "").Replace(".API", "").Replace("API", "");
                iserviceName = serviceName;
                if (serviceName.EndsWith("."))
                {
                    iserviceName += "Contracts";
                    serviceName += "Service";
                }
                else
                {
                    iserviceName += ".Contracts";
                    serviceName += ".Service";
                }
            }
           
            Console.WriteLine($"RegisterAutofac:serviceName={serviceName},iserviceName={iserviceName}");
            Assembly service = Assembly.Load(serviceName);
            Assembly iservice = Assembly.Load(iserviceName);
            containerBuilder.RegisterAssemblyTypes(service, iservice)
            .Where(t => t.FullName.EndsWith("Service") && !t.IsAbstract) //类名以service结尾，且类型不能是抽象的　
                .InstancePerLifetimeScope() //生命周期
                .AsImplementedInterfaces();
                //.PropertiesAutowired(); //属性注入


            //var controllerBaseType = typeof(ControllerBase);
            //containerBuilder.RegisterAssemblyTypes(typeof(Program).Assembly)
            //    .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
            //    .PropertiesAutowired();
            //.EnableClassInterceptors(); // 允许在Controller类上使用拦截器[Intercept(typeof(TestInterceptor))]
        }
    }
}

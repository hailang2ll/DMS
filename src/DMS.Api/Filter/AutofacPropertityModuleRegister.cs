using Autofac;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Api.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class AutofacPropertityModuleRegister : Autofac.Module
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();

        }
    }
}

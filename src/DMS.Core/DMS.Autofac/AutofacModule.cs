using Autofac;

namespace DMS.Autofac
{
    /// <summary>
    /// 
    /// </summary>
    public class AutofacModule : Module
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<UserInfoService>().As<IUserInfoService>();
            //builder.RegisterType<TestService>().As<ITestService>();
            //builder.RegisterType<MemberLoginService>().As<IMemberLoginService>();
        }
    }
}

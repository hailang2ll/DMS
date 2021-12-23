using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DMS.Common.Extensions;
namespace DMS.Test
{
    /// <summary>
    /// 基类
    /// </summary>
    public class Base_Test
    {
        public IConfiguration Configuration { get; }
        public Base_Test()
        {
            var builder = new ConfigurationBuilder()
            .AddJsonFile($"Configs/domain.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"Configs/sms.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"Configs/redis.json", optional: false, reloadOnChange: true)
            .AddAppSettingsFile($"appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();



            IServiceCollection services = new ServiceCollection();
            //services.AddCorsSetup();
        }
    }
}

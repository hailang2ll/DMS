using Autofac.Extensions.DependencyInjection;
using DMS.NLogs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DMS.XiaoYaoShuGe.WebSite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
               .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:20100");
                    webBuilder.UseNLog($"Configs/nlog.config");
                    webBuilder.UseStartup<Startup>();
                });
    }
}

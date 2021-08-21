using Autofac.Extensions.DependencyInjection;
using DMS.Log4net;
using DMS.NLogs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DMS.Sample31
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    //builder.SetBasePath("");
                    //builder.AddAppSettingsFile("");
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:5001");
                    webBuilder.UseNLog($"Configs/nlog.config");
                    webBuilder.UseLog4net($"Configs/log4net.config");
                    webBuilder.UseStartup<Startup>();
                }).UseServiceProviderFactory(new AutofacServiceProviderFactory());

    }
}

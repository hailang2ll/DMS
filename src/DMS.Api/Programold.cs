using Autofac.Extensions.DependencyInjection;
using DMS.NLogs.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace DMS.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Programold
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
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:20300");
                    webBuilder.UseNLog($"Configs/nlog.config");

                    webBuilder.UseStartup<Startup>()
                    .ConfigureLogging((hostingContext, builder) =>
                    {
                        //builder.AddLog4Net(Path.Combine(Directory.GetCurrentDirectory(), "Log4net.config"));
                    })
                 ; ;
                });
    }
}

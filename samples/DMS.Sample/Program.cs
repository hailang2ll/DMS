using DMS.Log4net;
using DMS.Redis.Configurations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NLogs;
using System.IO;

namespace DMS.Sample
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
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            //.ConfigureAppConfiguration(builder =>
            //{
            //    //builder.AddJsonFile("Configs/redis.json");
            //    //builder.AddJsonFile("Configs/domain.json");
            //    builder.AddRedisFile("Configs/redis.json", optional: false, reloadOnChange: true);
            //})

            .UseUrls("http://*:9870")
            .UseNLog($"Configs/nlog.config")
            .UseLog4net($"Configs/log4net.config")
            .UseStartup<Startup>();
    }
}

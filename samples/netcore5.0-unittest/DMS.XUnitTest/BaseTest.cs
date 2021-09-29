using DMS.Redis.Configurations;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.XUnitTest
{
    public class BaseTest
    {
        public IConfiguration Configuration { get; }
        public BaseTest()
        {
            var builder = new ConfigurationBuilder()
            .AddJsonFile($"Configs/redis.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
        }
    }
}

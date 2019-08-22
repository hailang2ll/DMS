using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Redis.Configurations
{
    public static class RedisConfigurationExtensions
    {
        public static IConfigurationBuilder AddRedisFile(this IConfigurationBuilder builder, string path)
        {
            return AddRedisFile(builder, provider: null, path: path, optional: false, reloadOnChange: false);
        }

        public static IConfigurationBuilder AddRedisFile(this IConfigurationBuilder builder, string path, bool optional)
        {
            return AddRedisFile(builder, provider: null, path: path, optional: optional, reloadOnChange: false);
        }

        public static IConfigurationBuilder AddRedisFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
        {
            return AddRedisFile(builder, provider: null, path: path, optional: optional, reloadOnChange: reloadOnChange);
        }

        public static IConfigurationBuilder AddRedisFile(this IConfigurationBuilder builder, IFileProvider provider, string path, bool optional, bool reloadOnChange)
        {
            var source = new JsonConfigurationSource()
            {
                FileProvider = provider,
                Path = path,
                Optional = optional,
                ReloadOnChange = reloadOnChange
            };
            builder.Add(source);
            AppConfig.Configuration = builder.Build();
            //AppConfig.RedisOption = AppConfig.Configuration.GetSection("RedisConfig").Get<RedisOption>();
            return builder;
        }
    }
}

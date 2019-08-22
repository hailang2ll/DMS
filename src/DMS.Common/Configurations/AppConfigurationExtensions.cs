using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;

namespace DMS.Common.Configurations
{
    public static class AppConfigurationExtensions
    {
        public static IConfigurationBuilder AddAppSettingsFile(this IConfigurationBuilder builder, string path)
        {
            return AddAppSettingsFile(builder, provider: null, path: path, basePath: null, optional: false, reloadOnChange: false);
        }

        public static IConfigurationBuilder AddAppSettingsFile(this IConfigurationBuilder builder, string path, bool optional)
        {
            return AddAppSettingsFile(builder, provider: null, path: path, basePath: null, optional: optional, reloadOnChange: false);
        }

        public static IConfigurationBuilder AddAppSettingsFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
        {
            return AddAppSettingsFile(builder, provider: null, path: path, basePath: null, optional: optional, reloadOnChange: reloadOnChange);
        }

        public static IConfigurationBuilder AddAppSettingsFile(this IConfigurationBuilder builder, string path, string basePath, bool optional, bool reloadOnChange)
        {
            return AddAppSettingsFile(builder, provider: null, path: path, basePath: basePath, optional: optional, reloadOnChange: reloadOnChange);
        }

        public static IConfigurationBuilder AddAppSettingsFile(this IConfigurationBuilder builder, IFileProvider provider, string path, string basePath, bool optional, bool reloadOnChange)
        {
            var source = new JsonConfigurationSource()
            {
                FileProvider = provider,
                Path = path,
                Optional = optional,
                ReloadOnChange = reloadOnChange
            };
            builder.Add(source);
            if (!string.IsNullOrEmpty(basePath))
                builder.SetBasePath(basePath);
            AppConfig.Configuration = builder.Build();
          
            return builder;
        }
    }
}

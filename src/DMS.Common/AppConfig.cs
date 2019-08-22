using Microsoft.Extensions.Configuration;

namespace DMS.Common
{
    public class AppConfig
    {
        public static IConfiguration Configuration { get; set; }
        public static IConfigurationSection GetSection(string name)
        {
            return Configuration?.GetSection(name);
        }
        public static string GetVaule(string name)
        {
            return GetSection(name).Value;
        }
        public static T GetVaule<T>(string name)
        {
            return GetSection(name).Get<T>();
        }
    }
}

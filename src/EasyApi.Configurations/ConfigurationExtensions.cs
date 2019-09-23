using Microsoft.Extensions.Configuration;

namespace EasyApi.Configurations
{
    public static class ConfigurationExtensions
    {
        public static AppInfo GetApplicationInfo(this IConfiguration config)
        {
            var name = config[ConfigurationKeys.AppTitleKey];
            var version = config[ConfigurationKeys.AppVersionKey];
            var environment = config[ConfigurationKeys.AppEnvironmentNameKey];

            return new AppInfo(name, version, environment);
        }
    }
}
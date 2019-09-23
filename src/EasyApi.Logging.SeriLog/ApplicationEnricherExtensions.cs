using Serilog;
using Serilog.Configuration;

namespace EasyApi.Logging.SeriLog
{
    public static class ApplicationEnricherExtensions
    {
        public const string ApplicationPropertyName = "Application";
        public const string EnvironmentPropertyName = "Environment";

        public static LoggerConfiguration WithApplicationName(this LoggerEnrichmentConfiguration config,
            string applicationName)
        {
            return config.WithProperty(ApplicationPropertyName, applicationName);
        }        
        
        public static LoggerConfiguration WithEnvironmentName(this LoggerEnrichmentConfiguration config,
            string environmentName)
        {
            return config.WithProperty(EnvironmentPropertyName, environmentName);
        }
    }
}
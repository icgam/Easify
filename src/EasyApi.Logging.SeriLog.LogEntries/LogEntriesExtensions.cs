using Microsoft.Extensions.Configuration;

namespace EasyApi.Logging.SeriLog.LogEntries
{
    public static class LogEntriesExtensions
    {
        public const string DefaultLogMessageTemplate =
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{MachineName}] [{EnvironmentUserName}] [{ProcessId}] [{UserName}] [{CorrelationId}] [{ThreadId}] [{Level}] {Message}{NewLine}{Exception}"
            ;

        public static IProvideTemplate UseLogEntries(this ILoggerConfiguration configurationServices, string token)
        {
            return new FluentLogEntriesSinkBuilder(configurationServices, token);
        }

        public static IBuildSink UseLogEntries(this ILoggerConfiguration configurationServices, IConfigurationSection config)
        {
            return new ConfigBasedLogEntriesSinkBuilder(configurationServices, config);
        }
    }
}
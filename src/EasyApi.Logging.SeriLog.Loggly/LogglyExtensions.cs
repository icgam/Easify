using System;
using Microsoft.Extensions.Configuration;

namespace EasyApi.Logging.SeriLog.Loggly
{
    public static class LogglyExtensions
    {
        public static ISetCustomerToken UseLoggly(this ILoggerConfiguration configurationServices, string serverUrl)
        {
            if (configurationServices == null) throw new ArgumentNullException(nameof(configurationServices));
            if (serverUrl == null) throw new ArgumentNullException(nameof(serverUrl));
            var serverUri = new Uri(serverUrl, UriKind.Absolute);

            return new FluentLogglySinkBuilder(configurationServices, serverUri);
        }

        public static IBuildSink UseLoggly(this ILoggerConfiguration configurationServices, IConfigurationSection config)
        {
            return new ConfigBasedLogglySinkBuilder(configurationServices, config);
        }
    }
}
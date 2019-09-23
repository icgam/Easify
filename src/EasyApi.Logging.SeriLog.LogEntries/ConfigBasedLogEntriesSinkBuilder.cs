using System;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Logentries;

namespace EasyApi.Logging.SeriLog.LogEntries
{
    public sealed class ConfigBasedLogEntriesSinkBuilder : IBuildSink
    {
        private readonly IConfigurationSection _config;
        private readonly ILoggerConfiguration _configurationServices;
        
        public ConfigBasedLogEntriesSinkBuilder(ILoggerConfiguration configurationServices,
            IConfigurationSection config)
        {
            _configurationServices = configurationServices ??
                                     throw new ArgumentNullException(nameof(configurationServices));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public LoggerConfiguration Build()
        {
            var options = _config.Get<LogEntriesConfiguration>();
            return _configurationServices.SinkConfiguration.Logentries(options.Token,
                outputTemplate: string.IsNullOrWhiteSpace(options.LogMessageTemplate)
                    ? LogEntriesExtensions.DefaultLogMessageTemplate
                    : options.LogMessageTemplate);
        }
    }
}
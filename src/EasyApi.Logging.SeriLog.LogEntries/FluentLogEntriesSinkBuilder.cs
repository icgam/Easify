using System;
using Serilog;
using Serilog.Sinks.Logentries;

namespace EasyApi.Logging.SeriLog.LogEntries
{
    public sealed class FluentLogEntriesSinkBuilder : IBuildSink, IProvideTemplate
    {
        private readonly string _apiToken;
        private readonly ILoggerConfiguration _configurationServices;
        private string _template;

        public FluentLogEntriesSinkBuilder(ILoggerConfiguration configurationServices, string apiToken)
        {
            if (string.IsNullOrWhiteSpace(apiToken))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiToken));

            _configurationServices = configurationServices ??
                                     throw new ArgumentNullException(nameof(configurationServices));
            _apiToken = apiToken;
        }

        public LoggerConfiguration Build()
        {
            return _configurationServices.SinkConfiguration.Logentries(_apiToken,
                outputTemplate: string.IsNullOrWhiteSpace(_template)
                    ? LogEntriesExtensions.DefaultLogMessageTemplate
                    : _template);
        }

        public IBuildSink WithTemplate(string template)
        {
            if (string.IsNullOrWhiteSpace(template))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(template));
            _template = template;
            return this;
        }
    }
}
using System;
using Serilog;

namespace EasyApi.Logging.SeriLog.Seq
{
    public sealed class FluentSeqSinkBuilder : ISetApiKey, IControlLogLevel, IBuildSink
    {
        private readonly ILoggerConfiguration _configurationServices;
        private bool _allowLogLevelToBeControlledRemotely;
        private string _apiKey;
        private readonly string _serverUrl;

        public FluentSeqSinkBuilder(ILoggerConfiguration configurationServices, string serverUrl)
        {
            if (string.IsNullOrWhiteSpace(serverUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serverUrl));

            _configurationServices = configurationServices ?? throw new ArgumentNullException(nameof(configurationServices));
            _serverUrl = serverUrl;
        }

        public IBuildSink EnableLogLevelControl()
        {
            _allowLogLevelToBeControlledRemotely = true;
            return this;
        }

        public LoggerConfiguration Build()
        {
            if (_allowLogLevelToBeControlledRemotely)
                return _configurationServices.SinkConfiguration.Seq(_serverUrl,
                    apiKey: _apiKey,
                    controlLevelSwitch: LoggingLevelSwitchProvider.Instance);

            return _configurationServices.SinkConfiguration.Seq(_serverUrl,
                apiKey: _apiKey);
        }

        public IControlLogLevel WithApiKey(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            return this;
        }
    }
}
using System;
using Loggly.Config;
using Serilog;

namespace EasyApi.Logging.SeriLog.Loggly
{
    public sealed class FluentLogglySinkBuilder: IControlLogLevel, IBuildSink, ISetCustomerToken, IConfigureLogBuffer
    {
        private readonly ILoggerConfiguration _configurationServices;
        private bool _allowLogLevelToBeControlledRemotely;
        private string _customerToken;
        private readonly Uri _serverUri;
        private string _bufferBaseFilename;
        private const int SslPort = 443;

        public FluentLogglySinkBuilder(ILoggerConfiguration configurationServices, Uri serverUri)
        {
            _configurationServices = configurationServices ?? throw new ArgumentNullException(nameof(configurationServices));
            _serverUri = serverUri ?? throw new ArgumentNullException(nameof(serverUri));
        }

        public IBuildSink EnableLogLevelControl()
        {
            _allowLogLevelToBeControlledRemotely = true;
            return this;
        }

        public IConfigureLogBuffer WithCustomerToken(string customerToken)
        {
            if (string.IsNullOrWhiteSpace(customerToken))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(customerToken));
            _customerToken = customerToken;
            return this;
        }

        public IControlLogLevel BufferLogsAt(string bufferBaseFilename)
        {
            if (string.IsNullOrWhiteSpace(bufferBaseFilename))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(bufferBaseFilename));
            _bufferBaseFilename = bufferBaseFilename;
            return this;
        }

        public LoggerConfiguration Build()
        {
            var config = LogglyConfig.Instance;
            config.CustomerToken = _customerToken;
            config.ApplicationName = $"MyApp-{_configurationServices.EnvironmentName}";

            config.Transport.EndpointHostname = _serverUri.Host;
            config.Transport.EndpointPort = SslPort;
            config.Transport.LogTransport = LogTransport.Https;

            if (_allowLogLevelToBeControlledRemotely)
                return _configurationServices.SinkConfiguration.Loggly(bufferBaseFilename: _bufferBaseFilename,
                    controlLevelSwitch: LoggingLevelSwitchProvider.Instance);

            return _configurationServices.SinkConfiguration.Loggly(bufferBaseFilename: _bufferBaseFilename);
        }
    }
}
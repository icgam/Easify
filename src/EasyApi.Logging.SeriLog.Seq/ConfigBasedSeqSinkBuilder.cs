using System;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace EasyApi.Logging.SeriLog.Seq
{
    public sealed class ConfigBasedSeqSinkBuilder : IBuildSink
    {
        private readonly ILoggerConfiguration _configurationServices;
        private readonly IConfigurationSection _config;

        public ConfigBasedSeqSinkBuilder(ILoggerConfiguration configurationServices, IConfigurationSection config)
        {
            if (configurationServices == null) throw new ArgumentNullException(nameof(configurationServices));

            _configurationServices = configurationServices;
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public LoggerConfiguration Build()
        {
            var options = _config.Get<SeqConfiguration>();

            if (options.AllowLogLevelToBeControlledRemotely)
                return _configurationServices.SinkConfiguration.Seq(options.ServerUrl,
                    apiKey: options.ApiKey,
                    controlLevelSwitch: LoggingLevelSwitchProvider.Instance);

            return _configurationServices.SinkConfiguration.Seq(options.ServerUrl,
                apiKey: options.ApiKey);
        }
    }
}

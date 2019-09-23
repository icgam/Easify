using System;
using Loggly.Config;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace EasyApi.Logging.SeriLog.Loggly
{
    public sealed class ConfigBasedLogglySinkBuilder : IBuildSink
    {
        private const int DefaultNumberOfEventsInSingleBatch = 50;
        private const int DefaultBatchPostingIntervalInSeconds = 5;
        private const int DefaultEventBodyLimitKb = 25;
        private const int DefaultRetainedInvalidPayloadsLimitMb = 128;
        private const int SslPort = 443;
        private const int KbToBytesMultiplier = 1024;
        private const int MbToBytesMultiplier = KbToBytesMultiplier * KbToBytesMultiplier;
        private readonly IConfigurationSection _config;
        private readonly ILoggerConfiguration _configurationServices;

        public ConfigBasedLogglySinkBuilder(ILoggerConfiguration configurationServices,
            IConfigurationSection config)
        {
            _configurationServices = configurationServices ?? throw new ArgumentNullException(nameof(configurationServices));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public LoggerConfiguration Build()
        {
            var options = _config.Get<LogglyConfiguration>();

            var config = LogglyConfig.Instance;
            var serverUri = new Uri(options.ServerUrl);
            config.CustomerToken = options.CustomerToken;
            config.ApplicationName =
                $"{_configurationServices.ApplicationName}-{_configurationServices.EnvironmentName}";

            config.Transport.EndpointHostname = serverUri.Host;
            config.Transport.EndpointPort = SslPort;
            config.Transport.LogTransport = LogTransport.Https;


            if (options.AllowLogLevelToBeControlledRemotely)
                return _configurationServices.SinkConfiguration.Loggly(bufferBaseFilename: options.BufferBaseFilename,
                    batchPostingLimit: options.NumberOfEventsInSingleBatch.GetValueOrDefault(
                        DefaultNumberOfEventsInSingleBatch),
                    period: TimeSpan.FromSeconds(
                        options.BatchPostingIntervalInSeconds.GetValueOrDefault(DefaultBatchPostingIntervalInSeconds)),
                    eventBodyLimitBytes: options.EventBodyLimitKb.GetValueOrDefault(DefaultEventBodyLimitKb) *
                                         KbToBytesMultiplier,
                    retainedInvalidPayloadsLimitBytes: options.RetainedInvalidPayloadsLimitMb.GetValueOrDefault(
                                                           DefaultRetainedInvalidPayloadsLimitMb) * MbToBytesMultiplier,
                    controlLevelSwitch: LoggingLevelSwitchProvider.Instance);

            return _configurationServices.SinkConfiguration.Loggly(bufferBaseFilename: options.BufferBaseFilename,
                batchPostingLimit: options.NumberOfEventsInSingleBatch.GetValueOrDefault(
                    DefaultNumberOfEventsInSingleBatch),
                period: TimeSpan.FromSeconds(
                    options.BatchPostingIntervalInSeconds.GetValueOrDefault(DefaultBatchPostingIntervalInSeconds)),
                eventBodyLimitBytes: options.EventBodyLimitKb.GetValueOrDefault(DefaultEventBodyLimitKb) *
                                     KbToBytesMultiplier,
                retainedInvalidPayloadsLimitBytes: options.RetainedInvalidPayloadsLimitMb.GetValueOrDefault(
                                                       DefaultRetainedInvalidPayloadsLimitMb) * MbToBytesMultiplier);
        }
    }
}
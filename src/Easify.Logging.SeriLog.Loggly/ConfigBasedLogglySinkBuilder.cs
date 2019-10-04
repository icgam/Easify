// This software is part of the Easify framework
// Copyright (C) 2019 Intermediate Capital Group
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
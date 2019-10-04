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
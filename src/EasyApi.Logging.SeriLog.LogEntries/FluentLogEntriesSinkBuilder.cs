// This software is part of the EasyApi framework
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
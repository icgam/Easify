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
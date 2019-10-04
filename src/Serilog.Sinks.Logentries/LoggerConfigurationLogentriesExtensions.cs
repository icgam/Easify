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

// Copyright 2014 Serilog Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.Logentries.Sinks.Logentries;

namespace Serilog.Sinks.Logentries
{
    /// <summary>
    ///     Adds the WriteTo.Logentries() extension method to <see cref="LoggerConfiguration" />.
    /// </summary>
    public static class LoggerConfigurationLogentriesExtensions
    {
        private const string DefaultLogentriesOutputTemplate = "{Timestamp:G} [{Level}] {Message}{NewLine}{Exception}";

        /// <summary>
        ///     Adds a sink that writes log events to the Logentries.com webservice.
        ///     Create a token TCP input for this on the logentries website.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="token">The token as found on the Logentries.com website.</param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="outputTemplate">
        ///     A message template describing the format used to write to the sink.
        ///     the default is "{Timestamp:G} [{Level}] {Message}{NewLine}{Exception}".
        /// </param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="useSsl">Specify if the connection needs to be secured.</param>
        /// <param name="batchPostingLimit">The maximum number of events to post in a single batch.</param>
        /// <param name="period">The time to wait between checking for event batches.</param>
        /// <returns>Logger configuration, allowing configuration to continue.</returns>
        /// <exception cref="ArgumentNullException">A required parameter is null.</exception>
        public static LoggerConfiguration Logentries(
            this LoggerSinkConfiguration loggerConfiguration,
            string token, bool useSsl = true,
            int batchPostingLimit = LogentriesSink.DefaultBatchPostingLimit,
            TimeSpan? period = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultLogentriesOutputTemplate,
            IFormatProvider formatProvider = null)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException("token");

            var defaultedPeriod = period ?? LogentriesSink.DefaultPeriod;

            return loggerConfiguration.Sink(
                new LogentriesSink(outputTemplate, formatProvider, token, useSsl, batchPostingLimit, defaultedPeriod),
                restrictedToMinimumLevel);
        }

        /// <summary>
        ///     Adds a sink that writes log events to the Logentries.com webservice.
        ///     Create a token TCP input for this on the logentries website.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="token">The token as found on the Logentries.com website.</param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="textFormatter">Used to format the logs sent to Logentries.</param>
        /// <param name="useSsl">Specify if the connection needs to be secured.</param>
        /// <param name="batchPostingLimit">The maximum number of events to post in a single batch.</param>
        /// <param name="period">The time to wait between checking for event batches.</param>
        /// <returns>Logger configuration, allowing configuration to continue.</returns>
        /// <exception cref="ArgumentNullException">A required parameter is null.</exception>
        public static LoggerConfiguration Logentries(
            this LoggerSinkConfiguration loggerConfiguration,
            string token,
            ITextFormatter textFormatter,
            bool useSsl = true,
            int batchPostingLimit = LogentriesSink.DefaultBatchPostingLimit,
            TimeSpan? period = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException("token");

            if (textFormatter == null)
                throw new ArgumentNullException("textFormatter");

            var defaultedPeriod = period ?? LogentriesSink.DefaultPeriod;

            return loggerConfiguration.Sink(
                new LogentriesSink(textFormatter, token, useSsl, batchPostingLimit, defaultedPeriod),
                restrictedToMinimumLevel);
        }
    }
}
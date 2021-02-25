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

namespace Easify.Logging.SeriLog.LogEntries
{
    public sealed class FluentLogEntriesSinkBuilder : ISinkBuilder, IProvideTemplate
    {
        private readonly string _apiToken;
        private readonly ISinkBuilderContext _sinkBuilderContext;
        private string _template;

        public FluentLogEntriesSinkBuilder(ISinkBuilderContext sinkBuilderContext, string apiToken)
        {
            if (string.IsNullOrWhiteSpace(apiToken))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiToken));

            _sinkBuilderContext = sinkBuilderContext ??
                                     throw new ArgumentNullException(nameof(sinkBuilderContext));
            _apiToken = apiToken;
        }

        public LoggerConfiguration Build()
        {
            return _sinkBuilderContext.LoggerConfiguration.WriteTo.Logentries(_apiToken,
                outputTemplate: string.IsNullOrWhiteSpace(_template)
                    ? LogEntriesExtensions.DefaultLogMessageTemplate
                    : _template);
        }

        public ISinkBuilder WithTemplate(string template)
        {
            if (string.IsNullOrWhiteSpace(template))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(template));
            _template = template;
            return this;
        }
    }
}
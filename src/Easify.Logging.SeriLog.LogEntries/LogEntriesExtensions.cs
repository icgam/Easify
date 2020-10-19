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

namespace Easify.Logging.SeriLog.LogEntries
{
    public static class LogEntriesExtensions
    {
        public const string DefaultLogMessageTemplate =
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{MachineName}] [{EnvironmentUserName}] [{ProcessId}] [{UserName}] [{CorrelationId}] [{ThreadId}] [{Level}] {Message}{NewLine}{Exception}";

        public static ISinkBuilderContext UseLogEntries(this ISinkBuilderContext sinkBuilderContext, string token, Action<IProvideTemplate> configure)
        {
            var builder = new FluentLogEntriesSinkBuilder(sinkBuilderContext, token);
            configure(builder);
            
            return builder.BuildAndCloneContext(sinkBuilderContext);
        }

        public static ISinkBuilderContext UseLogEntries(this ISinkBuilderContext sinkBuilderContext,
            IConfigurationSection config)
        {
            var builder = new LogEntriesSinkBuilder(sinkBuilderContext, config);
            
            return builder.BuildAndCloneContext(sinkBuilderContext);
        }
    }
}
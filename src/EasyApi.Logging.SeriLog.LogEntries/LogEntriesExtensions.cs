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

﻿using Microsoft.Extensions.Configuration;

namespace EasyApi.Logging.SeriLog.LogEntries
{
    public static class LogEntriesExtensions
    {
        public const string DefaultLogMessageTemplate =
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{MachineName}] [{EnvironmentUserName}] [{ProcessId}] [{UserName}] [{CorrelationId}] [{ThreadId}] [{Level}] {Message}{NewLine}{Exception}"
            ;

        public static IProvideTemplate UseLogEntries(this ILoggerConfiguration configurationServices, string token)
        {
            return new FluentLogEntriesSinkBuilder(configurationServices, token);
        }

        public static IBuildSink UseLogEntries(this ILoggerConfiguration configurationServices, IConfigurationSection config)
        {
            return new ConfigBasedLogEntriesSinkBuilder(configurationServices, config);
        }
    }
}
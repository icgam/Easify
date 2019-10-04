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
using System.IO;

namespace EasyApi.Logging.SeriLog.OptionsBuilder
{
    public sealed class SeriLogOptionsBuilder : IBuildSeriLogOptions, ISetFileSizeLimit, ISetLogFilesRetention,
        ISetLogsPath,
        ISetFlushToDiskInterval
    {
        private const int BytesInMegabyte = 1048576;
        private const int LogFileSizeLimit1000Mb = 1000 * BytesInMegabyte;
        private const int LogFilesToRetain = 10;
        private int? _flushToDiskIntervalInMs;

        private int _logFileSizeLimit = LogFileSizeLimit1000Mb;
        private int _logFilesToRetain = LogFilesToRetain;
        private string _path = string.Empty;

        public LoggingOptions Build()
        {
            if (_flushToDiskIntervalInMs.HasValue)
                return new LoggingOptions(_logFileSizeLimit, _logFilesToRetain,
                    TimeSpan.FromMilliseconds(_flushToDiskIntervalInMs.Value), _path);
            return new LoggingOptions(_logFileSizeLimit, _logFilesToRetain, null, _path);
        }

        public ISetLogFilesRetention SetMaxLogFileSizeInMbTo(int fileSizeInMegabytes)
        {
            if (fileSizeInMegabytes <= 0) throw new ArgumentOutOfRangeException(nameof(fileSizeInMegabytes));
            _logFileSizeLimit = fileSizeInMegabytes * BytesInMegabyte;
            return this;
        }

        public IBuildSeriLogOptions SaveLogsTo(string absoluteLogsPath)
        {
            if (string.IsNullOrWhiteSpace(absoluteLogsPath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(absoluteLogsPath));
            if (!Path.IsPathRooted(absoluteLogsPath))
                throw new ArgumentException(
                    $"'{absoluteLogsPath}' is NOT absolute path! Please provide rooted path like: 'D:\\Logs'",
                    nameof(absoluteLogsPath));

            _path = absoluteLogsPath;

            return this;
        }

        public ISetLogFilesRetention FlushToDiskEveryInMs(int intervalInMs)
        {
            if (intervalInMs <= 0) throw new ArgumentOutOfRangeException(nameof(intervalInMs));
            _flushToDiskIntervalInMs = intervalInMs;

            return this;
        }

        public ISetLogsPath RetainNumberOfLogFiles(int numberOfLogFilesToRetain)
        {
            if (numberOfLogFilesToRetain <= 0) throw new ArgumentOutOfRangeException(nameof(numberOfLogFilesToRetain));
            _logFilesToRetain = numberOfLogFilesToRetain;
            return this;
        }
    }
}
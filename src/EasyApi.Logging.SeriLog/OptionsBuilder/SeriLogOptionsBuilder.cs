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
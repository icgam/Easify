using System;

namespace EasyApi.Logging.SeriLog.OptionsBuilder
{
    public sealed class LoggingOptions
    {
        public LoggingOptions(int logFileSizeLimitInBytes, int logFilesToRetain, TimeSpan? flushToDiskInterval, string logsPath)
        {
            LogFileSizeLimitInBytes = logFileSizeLimitInBytes;
            LogFilesToRetain = logFilesToRetain;
            FlushToDiskInterval = flushToDiskInterval;
            LogsPath = logsPath;
        }

        public int LogFileSizeLimitInBytes { get; }
        public int LogFilesToRetain { get; }
        public TimeSpan? FlushToDiskInterval { get; }
        public string LogsPath { get; }
        public bool LogsPathSet => string.IsNullOrWhiteSpace(LogsPath) == false;
    }
}
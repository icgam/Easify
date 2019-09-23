using System;
using Serilog;
using Serilog.Events;

namespace EasyApi.AspNetCore.Logging.SeriLog
{
    // TODO: Need to move out
    public static class SeriLogExtensions
    {
        public static LoggerConfiguration IgnoreSystemLogs(this LoggerConfiguration loggerConfiguration)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException(nameof(loggerConfiguration));

            return loggerConfiguration
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Error);
        }
    }
}
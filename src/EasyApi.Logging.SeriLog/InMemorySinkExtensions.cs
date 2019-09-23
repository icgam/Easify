using System;
using Serilog;
using Serilog.Configuration;
using Serilog.Formatting.Display;

namespace EasyApi.Logging.SeriLog
{
    public static class InMemorySinkExtensions
    {
        public static LoggerConfiguration InMemoryCache(this LoggerSinkConfiguration sinkConfiguration,
            string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}",
            IFormatProvider formatProvider = null)
        {
            if (sinkConfiguration == null)
                throw new ArgumentNullException(nameof(sinkConfiguration));
            var templateTextFormatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
            return sinkConfiguration.Sink(new InMemorySink(templateTextFormatter));
        }
    }
}
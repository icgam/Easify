using System;
using System.IO;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace EasyApi.Logging.SeriLog
{
    public sealed class InMemorySink : ILogEventSink
    {
        private readonly ITextFormatter _textFormatter;

        public InMemorySink(ITextFormatter textFormatter)
        {
            _textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));

            var renderSpace = new StringWriter();
            _textFormatter.Format(logEvent, renderSpace);

            var levelName = Enum.GetName(typeof(LogEventLevel), logEvent.Level);

            if (logEvent.Level > LogEventLevel.Warning)
                LogsStore.Instance.Errors.Add(
                    new LogMessage(logEvent.Timestamp.LocalDateTime, levelName, renderSpace.ToString()));

            LogsStore.Instance.Logs.Add(new LogMessage(logEvent.Timestamp.LocalDateTime, levelName,
                renderSpace.ToString()));
        }
    }
}
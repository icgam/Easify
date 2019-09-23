using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace EasyApi.AspNetCore.UnitTests.Helpers
{
    public sealed class InMemoryLogger<T> : ILogger<T>, ILogger where T : class
    {
        private static readonly string LoglevelPadding = ": ";
        private static readonly string MessagePadding;
        private static readonly string NewLineWithMessagePadding;
        private readonly Object _lock = new Object();
        private Func<string, LogLevel, bool> _filter;
        private readonly List<string> _logs = new List<string>();
        private StringBuilder _logBuilder;

        static InMemoryLogger()
        {
            var logLevelString = GetLogLevelString(LogLevel.Information);
            MessagePadding = new string(' ', logLevelString.Length + LoglevelPadding.Length);
            NewLineWithMessagePadding = Environment.NewLine + MessagePadding;
        }

        public InMemoryLogger(string name, Func<string, LogLevel, bool> filter)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Filter = filter ?? ((category, logLevel) => true);
        }
        
        public Func<string, LogLevel, bool> Filter
        {
            get => _filter;
            set => _filter = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Name { get; }

        public List<string> Logs
        {
            get
            {
                lock (_lock)
                {
                    return _logs;
                }
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                WriteMessage(logLevel, Name, eventId.Id, message, exception);
            }
        }

        private void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception exception)
        {
            var logBuilder = _logBuilder;
            _logBuilder = null;

            if (logBuilder == null)
            {
                logBuilder = new StringBuilder();
            }

            logBuilder.Append(LoglevelPadding);
            logBuilder.Append(logName);
            logBuilder.Append("[");
            logBuilder.Append(eventId);
            logBuilder.AppendLine("]");

            if (!string.IsNullOrEmpty(message))
            {
                logBuilder.Append(MessagePadding);

                var len = logBuilder.Length;
                logBuilder.AppendLine(message);
                logBuilder.Replace(Environment.NewLine, NewLineWithMessagePadding, len, message.Length);
            }

            if (exception != null)
            {
                logBuilder.AppendLine(exception.ToString());
            }

            if (logBuilder.Length > 0)
            {
                var logLevelString = GetLogLevelString(logLevel);

                lock (_lock)
                {
                    Logs.Add($"{logLevelString} {logBuilder}");
                }
            }

            logBuilder.Clear();
            if (logBuilder.Capacity > 1024)
            {
                logBuilder.Capacity = 1024;
            }
            _logBuilder = logBuilder;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel == LogLevel.None)
            {
                return false;
            }

            return Filter(Name, logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            return ConsoleLogScope.Push(Name, state);
        }

        private static string GetLogLevelString(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return "Trace";
                case LogLevel.Debug:
                    return "Debug";
                case LogLevel.Information:
                    return "Information";
                case LogLevel.Warning:
                    return "Warning";
                case LogLevel.Error:
                    return "Error";
                case LogLevel.Critical:
                    return "Critical";
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }
    }
}
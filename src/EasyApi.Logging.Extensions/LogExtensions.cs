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

﻿using System;
using Microsoft.Extensions.Logging;

namespace EasyApi.Logging.Extensions
{
    public static class LogExtensions
    {
        public static void LogTrace(this ILogger logger, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            logger.Log(LogLevel.Trace, () => new FormatMessageHandlerFormatter(formatMessageCallback).ToString(), exception);
        }

        public static void LogTrace(this ILogger logger, Action<FormatMessageHandler> formatMessageCallback)
        {
            logger.Log(LogLevel.Trace, () => new FormatMessageHandlerFormatter(formatMessageCallback).ToString());
        }

        public static void LogTrace(this ILogger logger, Func<string> messageFactory, Exception exception)
        {
            logger.Log(LogLevel.Trace, messageFactory, exception);
        }

        public static void LogTrace(this ILogger logger, Func<string> messageFactory)
        {
            logger.Log(LogLevel.Trace, messageFactory);
        }

        public static void LogDebug(this ILogger logger, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            logger.Log(LogLevel.Debug, () => new FormatMessageHandlerFormatter(formatMessageCallback).ToString(), exception);
        }

        public static void LogDebug(this ILogger logger, Action<FormatMessageHandler> formatMessageCallback)
        {
            logger.Log(LogLevel.Debug, () => new FormatMessageHandlerFormatter(formatMessageCallback).ToString());
        }

        public static void LogDebug(this ILogger logger, Func<string> messageFactory, Exception exception)
        {
            logger.Log(LogLevel.Debug, messageFactory, exception);
        }

        public static void LogDebug(this ILogger logger, Func<string> messageFactory)
        {
            logger.Log(LogLevel.Debug, messageFactory);
        }

        public static void LogInformation(this ILogger logger, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            logger.Log(LogLevel.Information, () => new FormatMessageHandlerFormatter(formatMessageCallback).ToString(), exception);
        }

        public static void LogInformation(this ILogger logger, Action<FormatMessageHandler> formatMessageCallback)
        {
            logger.Log(LogLevel.Information, () => new FormatMessageHandlerFormatter(formatMessageCallback).ToString());
        }
        
        public static void LogInformation(this ILogger logger, Func<string> messageFactory, Exception exception)
        {
            logger.Log(LogLevel.Information, messageFactory, exception);
        }

        public static void LogInformation(this ILogger logger, Func<string> messageFactory)
        {
            logger.Log(LogLevel.Information, messageFactory);
        }

        public static void LogWarning(this ILogger logger, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            logger.Log(LogLevel.Warning, () => new FormatMessageHandlerFormatter(formatMessageCallback).ToString(), exception);
        }

        public static void LogWarning(this ILogger logger, Action<FormatMessageHandler> formatMessageCallback)
        {
            logger.Log(LogLevel.Warning, () => new FormatMessageHandlerFormatter(formatMessageCallback).ToString());
        }

        public static void LogWarning(this ILogger logger, Func<string> messageFactory, Exception exception)
        {
            logger.Log(LogLevel.Warning, messageFactory, exception);
        }

        public static void LogWarning(this ILogger logger, Func<string> messageFactory)
        {
            logger.Log(LogLevel.Warning, messageFactory);
        }

        public static void LogError(this ILogger logger, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            logger.Log(LogLevel.Error, () => new FormatMessageHandlerFormatter(formatMessageCallback).ToString(), exception);
        }

        public static void LogError(this ILogger logger, Action<FormatMessageHandler> formatMessageCallback)
        {
            logger.Log(LogLevel.Error, () => new FormatMessageHandlerFormatter(formatMessageCallback).ToString());
        }

        public static void LogError(this ILogger logger, Func<string> messageFactory, Exception exception)
        {
            logger.Log(LogLevel.Error, messageFactory, exception);
        }

        public static void LogError(this ILogger logger, Func<string> messageFactory)
        {
            logger.Log(LogLevel.Error, messageFactory);
        }
        
        public static void LogCritical(this ILogger logger, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            logger.Log(LogLevel.Critical, () => new FormatMessageHandlerFormatter(formatMessageCallback).ToString(), exception);
        }

        public static void LogCritical(this ILogger logger, Action<FormatMessageHandler> formatMessageCallback)
        {
            logger.Log(LogLevel.Critical, () => new FormatMessageHandlerFormatter(formatMessageCallback).ToString());
        }

        public static void LogCritical(this ILogger logger, Func<string> messageFactory, Exception exception)
        {
            logger.Log(LogLevel.Critical, messageFactory, exception);
        }

        public static void LogCritical(this ILogger logger, Func<string> messageFactory)
        {
            logger.Log(LogLevel.Critical, messageFactory);
        }

        private static void Log(this ILogger logger, LogLevel logLevel, Func<string> messageFactory,
            Exception exception = null)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            if (!logger.IsEnabled(logLevel))
                return;

            if (messageFactory == null) throw new ArgumentNullException(nameof(messageFactory));

            switch (logLevel)
            {
                case LogLevel.Trace:
                    logger.LogTrace(0, exception, messageFactory());
                    break;
                case LogLevel.Debug:
                    logger.LogDebug(0, exception, messageFactory());
                    break;
                case LogLevel.Information:
                    logger.LogInformation(0, exception, messageFactory());
                    break;
                case LogLevel.Warning:
                    logger.LogWarning(0, exception, messageFactory());
                    break;
                case LogLevel.Error:
                    logger.LogError(0, exception, messageFactory());
                    break;
                case LogLevel.Critical:
                    logger.LogCritical(0, exception, messageFactory());
                    break;
            }
        }

        private sealed class FormatMessageHandlerFormatter
        {
            private volatile string _result = string.Empty;
            private readonly Action<FormatMessageHandler> _handler;

            public FormatMessageHandlerFormatter(Action<FormatMessageHandler> handler)
            {
                _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            }

            public override string ToString()
            {
                _handler(FormatMessage);
                return _result;
            }
            private string FormatMessage(string format, params object[] args)
            {
                if (format == null) throw new ArgumentNullException(nameof(format));

                _result = string.Format(format, args);
                return _result;
            }
        }
    }
}
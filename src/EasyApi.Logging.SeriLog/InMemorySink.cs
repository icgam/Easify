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
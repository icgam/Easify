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

using Easify.Extensions;

namespace Easify.Logging
{
    public sealed class LogsStore
    {
        private const int BufferSize = 50;
        private const int ErrorsBufferSize = 5;
        private static volatile LogsStore _instance;
        private static readonly object SyncRoot = new object();

        private LogsStore()
        {
            Logs = new CircularBuffer<LogMessage>(BufferSize);
            Errors = new CircularBuffer<LogMessage>(ErrorsBufferSize);
        }

        public static LogsStore Instance
        {
            get
            {
                if (_instance == null)
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new LogsStore();
                    }

                return _instance;
            }
        }

        public CircularBuffer<LogMessage> Logs { get; }

        public CircularBuffer<LogMessage> Errors { get; }
    }
}
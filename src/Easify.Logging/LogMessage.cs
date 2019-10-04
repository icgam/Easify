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

namespace Easify.Logging
{
    public sealed class LogMessage
    {
        public LogMessage(DateTime loggedAt, string level, string message)
        {
            LoggedAt = loggedAt;
            Level = level;
            Message = message;
        }

        public DateTime LoggedAt { get; }
        public string Level { get; }
        public string Message { get; }
    }
}
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

 using Serilog;
using Serilog.Events;

namespace EasyApi.Sample.WebAPI.IntegrationTests.Helpers
{
    public static class LoggerExtensions
    {
        public static string GetActiveLogLevel(this ILogger logger)
        {
            if (Log.Logger.IsEnabled(LogEventLevel.Verbose))
                return LogEventLevel.Verbose.ToString();
            if (Log.Logger.IsEnabled(LogEventLevel.Debug))
                return LogEventLevel.Debug.ToString();
            if (Log.Logger.IsEnabled(LogEventLevel.Information))
                return LogEventLevel.Information.ToString();
            if (Log.Logger.IsEnabled(LogEventLevel.Warning))
                return LogEventLevel.Warning.ToString();
            if (Log.Logger.IsEnabled(LogEventLevel.Error))
                return LogEventLevel.Error.ToString();

            return LogEventLevel.Fatal.ToString();
        }
    }
}
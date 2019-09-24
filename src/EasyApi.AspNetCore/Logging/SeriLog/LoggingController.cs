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
using System.Collections.Generic;
using System.Linq;
using EasyApi.Logging;
using EasyApi.Logging.SeriLog;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;

namespace EasyApi.AspNetCore.Logging.SeriLog
{
    [Route("diagnostics/logs")]
    public class LoggingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new OkObjectResult(new
            {
                LoggingLevel =
                    $"Current minimum level of logging is set to: '{Enum.GetName(typeof(LogEventLevel), LoggingLevelSwitchProvider.Instance.MinimumLevel)}'. " +
                    "You can change logging level by posting to THIS url appended with valid log level like '%url%/Debug'.",
                SupportedLoggingLevels = GetSupportedLoggingLevels(),
                Logs = new
                {
                    LatestErrors = LogsStore.Instance.Errors.OrderByDescending(e => e.LoggedAt).ToList(),
                    Messages = LogsStore.Instance.Logs.OrderByDescending(e => e.LoggedAt).ToList()
                }
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] ConfigureLoggingRequest request)
        {
            try
            {
                var minimumLoggingLevel = (LogEventLevel) Enum.Parse(typeof(LogEventLevel), request.Logginglevel);
                LoggingLevelSwitchProvider.Instance.MinimumLevel = minimumLoggingLevel;
                return Ok();
            }
            catch
            {
                return BadRequest(
                    $"'{request.Logginglevel}' is NOT a valid logging level! Valid logging levels are: {string.Join(", ", GetSupportedLoggingLevels())}");
            }
        }

        private static List<string> GetSupportedLoggingLevels()
        {
            return new List<string>
            {
                Enum.GetName(typeof(LogEventLevel), LogEventLevel.Verbose),
                Enum.GetName(typeof(LogEventLevel), LogEventLevel.Debug),
                Enum.GetName(typeof(LogEventLevel), LogEventLevel.Information),
                Enum.GetName(typeof(LogEventLevel), LogEventLevel.Warning),
                Enum.GetName(typeof(LogEventLevel), LogEventLevel.Error),
                Enum.GetName(typeof(LogEventLevel), LogEventLevel.Fatal)
            };
        }
    }
}
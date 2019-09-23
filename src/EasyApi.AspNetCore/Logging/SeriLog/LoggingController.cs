using System;
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
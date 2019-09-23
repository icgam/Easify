using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EasyApi.Sample.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class LogsController : Controller
    {
        private ILogger<LogsController> Log { get; }

        public LogsController(ILogger<LogsController> logger)
        {
            Log = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult Get()
        {
            Log.LogTrace("Trace");
            Log.LogDebug("Debug");
            Log.LogInformation("Information");
            Log.LogWarning("Warning");
            Log.LogError("Error");
            Log.LogCritical("Critical");

            return Ok();
        }
    }
}
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Easify.Sample.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class SampleController : Controller
    {
        public SampleController(ILogger<SampleController> logger)
        {
            Log = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private ILogger<SampleController> Log { get; }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
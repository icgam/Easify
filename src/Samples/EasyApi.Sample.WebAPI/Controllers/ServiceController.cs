using System;
using EasyApi.Sample.WebAPI.Core;
using Microsoft.AspNetCore.Mvc;

namespace EasyApi.Sample.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ServiceController : Controller
    {
        private readonly IMyService _service;

        public ServiceController(IMyService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            _service = service;
        }

        [HttpGet("{data}")]
        public string Get(string data)
        {
            return _service.Process(data);
        }
    }
}

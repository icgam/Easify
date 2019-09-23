using System;
using EasyApi.Sample.WebAPI.Domain;
using Microsoft.AspNetCore.Mvc;

namespace EasyApi.Sample.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ErrorsController : Controller
    {
        [HttpGet("{data}")]
        public IActionResult Get(string data)
        {
            if (data == "throwFriendly")
                throw new OurApplicationException();
            if (data == "throwUnfriendly")
                throw new Exception("Very bad exception!");
            if (data == "throwThirdParty")
                throw new ThirdPartyPluginFailedException();
            if (data == "throwHierarchy")
            {
                var leafException1 = new OurApplicationException("My friedly leaf1!");
                var leafException2 = new Exception("Security critical super secret!");
                var leafException4 = new ThirdPartyPluginFailedException();
                var leafException3 = new Exception("Security critical super secret #2!", leafException4);
                var aggregate = new AggregateException("Should not see me! (aggregate)", leafException1, leafException2,
                    leafException3);
                throw new OurApplicationException("Admin root thrown!", aggregate);
            }

            return new ObjectResult(data);
        }
    }
}
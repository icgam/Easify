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
using Easify.Sample.WebAPI.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Easify.Sample.WebAPI.Controllers
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
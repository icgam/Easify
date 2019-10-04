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
using Easify.Sample.WebAPI.Core;
using Microsoft.AspNetCore.Mvc;

namespace Easify.Sample.WebAPI.Controllers
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
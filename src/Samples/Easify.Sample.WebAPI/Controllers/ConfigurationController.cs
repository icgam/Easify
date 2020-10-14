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
using Easify.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Easify.Sample.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ConfigurationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<Application> _applicationAccessor;

        public ConfigurationController(IConfiguration configuration, IOptions<Application> applicationAccessor)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _applicationAccessor = applicationAccessor ?? throw new ArgumentNullException(nameof(applicationAccessor));
        }
        
        [HttpGet("environment/{name}")]
        public IActionResult GetValidEnvironmentConfig(string name)
        {
            var configItem = _configuration[name];
            if (string.IsNullOrWhiteSpace(configItem))
                return NotFound();
            
            return Ok(configItem);
        } 
    }
}
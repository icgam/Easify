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
using System.Collections.Generic;
using System.Threading.Tasks;
using Easify.Sample.WebAPI.Core;
using Microsoft.AspNetCore.Mvc;

namespace Easify.Sample.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ValuesConsumerController : Controller
    {
        private readonly IValuesClient _valuesClient;

        public ValuesConsumerController(IValuesClient valuesClient)
        {
            _valuesClient = valuesClient ?? throw new ArgumentNullException(nameof(valuesClient));
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            return await _valuesClient.GetValuesAsync();
        }

        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            return await _valuesClient.GetValueAsync(id);
        }

        [HttpPost]
        public async Task Post([FromBody] string value)
        {
            await _valuesClient.GetValuesAsync();
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] string value)
        {
            await _valuesClient.GetValuesAsync();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _valuesClient.GetValuesAsync();
        }
    }
}
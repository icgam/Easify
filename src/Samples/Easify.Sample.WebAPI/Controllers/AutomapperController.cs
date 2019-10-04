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
using Easify.Extensions;
using Easify.Sample.WebAPI.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Easify.Sample.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AutomapperController : Controller
    {
        private readonly ITypeMapper _mapper;

        public AutomapperController(ITypeMapper mapper)
        {
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _mapper = mapper;
        }

        [HttpGet("person/{firstname}/{lastname}")]
        public IActionResult GetPerson(string firstname, string lastname)
        {
            var personToMap = new PersonEntity
            {
                FirstName = firstname,
                LastName = lastname
            };

            var person = _mapper.Map<PersonDO>(personToMap);
            return new ObjectResult(person);
        }

        [HttpGet("asset/{assetid}")]
        public IActionResult GetAsset(string assetid)
        {
            var assetToMap = new AssetEntity
            {
                Id = assetid
            };

            var asset = _mapper.Map<AssetDO>(assetToMap);
            return new ObjectResult(asset);
        }
    }
}
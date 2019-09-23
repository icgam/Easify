using System;
using EasyApi.Extensions;
using EasyApi.Sample.WebAPI.Domain;
using Microsoft.AspNetCore.Mvc;

namespace EasyApi.Sample.WebAPI.Controllers
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
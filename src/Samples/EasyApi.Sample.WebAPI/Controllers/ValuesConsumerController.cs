using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyApi.Sample.WebAPI.Core;
using Microsoft.AspNetCore.Mvc;

namespace EasyApi.Sample.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ValuesConsumerController : Controller
    {
        private readonly IValuesClient _valuesClient;

        public ValuesConsumerController(IValuesClient valuesClient)
        {
            if (valuesClient == null) throw new ArgumentNullException(nameof(valuesClient));
            _valuesClient = valuesClient;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            return await _valuesClient.GetValues();
        }

        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            return await _valuesClient.GetValue(id);
        }

        [HttpPost]
        public async Task Post([FromBody] string value)
        {
            await _valuesClient.GetValues();
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] string value)
        {
            await _valuesClient.GetValues();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _valuesClient.GetValues();
        }
    }
}
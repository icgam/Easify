using System.Threading.Tasks;
using EasyApi.Sample.WebAPI.Domain;
using Microsoft.AspNetCore.Mvc;

namespace EasyApi.Sample.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public sealed class ValidationController : Controller
    {
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StoreDocumentsRequest request)
        {
            return await Task.FromResult(Ok());
        }
    }
}

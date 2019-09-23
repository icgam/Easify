using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace EasyApi.Sample.WebAPI.IntegrationTests.Helpers
{
    public sealed class JsonContent : StringContent
    {
        public JsonContent(object content) :
            base(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json")
        { }
    }
}

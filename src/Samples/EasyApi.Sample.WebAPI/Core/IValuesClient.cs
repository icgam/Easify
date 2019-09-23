using System.Collections.Generic;
using System.Threading.Tasks;
using EasyApi.RestEase.Client;
using RestEase;

namespace EasyApi.Sample.WebAPI.Core
{
    public interface IValuesClient : IRestClient
    {
        [Get("api/values")]
        Task<IEnumerable<string>> GetValues();

        [Get("api/values/{id}")]
        Task<string> GetValue([Path] int id);

        [Get("api/values/{id}")]
        Task PostValue([Path] int id, [Body] string value);

        [Get("api/values/{id}")]
        Task PutValue([Path] int id, [Body] string value);

        [Get("api/values/{id}")]
        Task DeleteValue([Path] int id);
    }
}

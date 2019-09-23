using EasyApi.Http;
using RestEase;

namespace EasyApi.RestEase.Client
{
    // TODO: Is this worth to have a separate simple class
    // TODO: Extend this with the with extra properties for security & ...
    public interface IRestClient
    {
        [Header(HttpHeaders.HttpRequestId)]
        string CorrelationId { get; set; }
    }
}
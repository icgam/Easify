using EasyApi.RestEase.Client;

namespace EasyApi.RestEase
{
    public interface IRestClientBuilder
    {
        T Build<T>(string baseUrl) where T : IRestClient;
    }
}
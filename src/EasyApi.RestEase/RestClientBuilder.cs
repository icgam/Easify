using System;
using EasyApi.Http;
using EasyApi.RestEase.Client;
using RestEase;

namespace EasyApi.RestEase
{
    // TODO: Should be revisited in shadow of the v2.1 Factory and Polly
    public class RestClientBuilder : IRestClientBuilder
    {
        private readonly IRequestContext _requestContext;

        public RestClientBuilder(IRequestContext requestContext)
        {
            _requestContext = requestContext ?? throw new ArgumentNullException(nameof(requestContext));
        }

        public T Build<T>(string baseUrl) where T : IRestClient
        {
            if (baseUrl == null) throw new ArgumentNullException(nameof(baseUrl));

            var httpContext = new Uri(baseUrl).ToHttpContext(m => m.EnableDefaultCredential());

            var client = RestClient.For<T>(httpContext);
            client.CorrelationId = _requestContext.CorrelationId;

            return client;
        }
    }
}
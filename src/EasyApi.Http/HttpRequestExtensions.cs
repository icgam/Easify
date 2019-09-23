using System;
using System.Net.Http;

namespace EasyApi.Http
{
    public static class HttpRequestExtensions
    {
        public static HttpClient AddRequestIdToHeader(this HttpClient httpClient,
            string requestId)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(requestId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(requestId));

            httpClient.DefaultRequestHeaders.Add(HttpHeaders.HttpRequestId, requestId);
            return httpClient;
        }
    }
}

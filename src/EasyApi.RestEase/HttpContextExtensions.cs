using System;
using System.Net;
using System.Net.Http;

namespace EasyApi.RestEase
{
    public static class HttpContextExtensions
    {
        public static HttpClient ToHttpContext(this Uri baseUri, Action<HttpContextOptions> action = null)
        {
            if (baseUri == null) throw new ArgumentNullException(nameof(baseUri));

            var httpContextOptions = new HttpContextOptions();
            action?.Invoke(httpContextOptions);

            var handler = new HttpClientHandler();

            if (httpContextOptions.IsDefaultCredentialEnabled)
                handler.Credentials = CredentialCache.DefaultNetworkCredentials;

            return new HttpClient(handler) {BaseAddress = baseUri};
        }
    }
}
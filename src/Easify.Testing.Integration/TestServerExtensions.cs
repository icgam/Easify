using System;
using Easify.RestEase.Client;
using Microsoft.AspNetCore.TestHost;
using RestEase;

namespace Easify.Testing.Integration
{
    public static class TestServerExtensions
    {
        public static T CreateClient<T>(this TestServer server) where T : IRestClient
        {
            if (server == null) throw new ArgumentNullException(nameof(server));

            return RestClient.For<T>(server.CreateClient());
        }
    }
}
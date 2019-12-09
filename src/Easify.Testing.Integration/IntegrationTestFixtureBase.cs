using System;
using System.IO;
using Easify.RestEase.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace Easify.Testing.Integration
{
    public class IntegrationTestFixtureBase<TStartup> : FixtureBase where TStartup : class
    {
        public TestServer GetServer(Action<TestServer> setup = null)
        {
            var builder = CreateBuilder();
            builder.UseStartup<TStartup>();

            var server = new TestServer(builder);

            setup?.Invoke(server);
            return server;
        }

        public T CreateClientFromServer<T>(Action<IServiceProvider> setup = null) where T : IRestClient
        {
            var server = GetServer(s => { setup?.Invoke(s.Host.Services); });
            return server.CreateClient<T>();
        }

        protected virtual IWebHostBuilder CreateBuilder()
        {
            return new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = context.HostingEnvironment;
                    env.EnvironmentName = "Development";

                    builder.SetBasePath(env.ContentRootPath)
                        .AddJsonFile("appsettings.json", false, true);
                });
        }
    }
}
using System;
using Microsoft.Extensions.DependencyInjection;

namespace EasyApi.Sample.WebAPI.IntegrationTests.Helpers
{
    public sealed class TestServerOptions
    {
        public bool EnableLoggingToFile { get; set; } = false;
        public string Environment { get; set; } = "Development";
        public Action<IServiceCollection> ConfigureServices { get; set; } = sc => {};
    }
}
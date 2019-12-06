using Easify.RestEase.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Easify.Testing.Integration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFakeRestClient<T>(this IServiceCollection services)
            where T : class, IRestClient
        {
            services.AddSingleton(Substitute.For<T>());
            return services;
        }

        public static IServiceCollection AddFakeAsSingleton<T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton(Substitute.For<T>());
            return services;
        }
    }
}
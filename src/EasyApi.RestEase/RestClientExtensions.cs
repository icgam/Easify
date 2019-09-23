using System;
using EasyApi.RestEase.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace EasyApi.RestEase
{
    public static class RestClientExtensions
    {
        public static IServiceCollection AddRestClient<TClient, TConfiguration>(this IServiceCollection services,
            Func<TConfiguration, string> urlProvider)
            where TClient : class, IRestClient
            where TConfiguration : class, new()
        {
            if (urlProvider == null) throw new ArgumentNullException(nameof(urlProvider));

            services.TryAddTransient<IRestClientBuilder, RestClientBuilder>();
            services.AddTransient(m =>
            {
                var config = m.GetService<IOptions<TConfiguration>>();
                var builder = m.GetService<IRestClientBuilder>();

                return builder.Build<TClient>(urlProvider(config.Value));
            });

            return services;
        }

        public static IServiceCollection AddRestClient<TClient>(this IServiceCollection services,
            Func<string> urlProvider)
            where TClient : class, IRestClient
        {
            if (urlProvider == null) throw new ArgumentNullException(nameof(urlProvider));

            services.TryAddTransient<IRestClientBuilder, RestClientBuilder>();
            services.AddTransient(m =>
            {
                var builder = m.GetService<IRestClientBuilder>();
                
                return builder.Build<TClient>(urlProvider());
            });

            return services;
        }

        public static IServiceCollection TryAddRestClient<TClient, TConfiguration>(this IServiceCollection services,
            Func<TConfiguration, string> urlProvider)
            where TClient : class, IRestClient
            where TConfiguration : class, new()
        {
            if (urlProvider == null) throw new ArgumentNullException(nameof(urlProvider));

            services.TryAddTransient<IRestClientBuilder, RestClientBuilder>();
            services.TryAddTransient(m =>
            {
                var config = m.GetService<IOptions<TConfiguration>>();
                var builder = m.GetService<IRestClientBuilder>();

                return builder.Build<TClient>(urlProvider(config.Value));
            });
 
            return services;
        }

        public static IServiceCollection TryAddRestClient<TClient>(this IServiceCollection services,
            Func<string> urlProvider)
            where TClient : class, IRestClient
        {
            if (urlProvider == null) throw new ArgumentNullException(nameof(urlProvider));

            services.TryAddTransient<IRestClientBuilder, RestClientBuilder>();
            services.TryAddTransient(m =>
            {
                var builder = m.GetService<IRestClientBuilder>();

                return builder.Build<TClient>(urlProvider());
            });

            return services;
        }
    }
}
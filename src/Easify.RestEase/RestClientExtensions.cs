// This software is part of the Easify framework
// Copyright (C) 2019 Intermediate Capital Group
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Easify.RestEase.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Easify.RestEase
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
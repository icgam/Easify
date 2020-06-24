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
using System.Net.Http.Headers;
using Easify.Http;
using Easify.RestEase.Client;
using RestEase;

namespace Easify.RestEase
{
    // TODO: Should be revisited in shadow of the v2.1 Factory and Polly
    public sealed class RestClientBuilder : IRestClientBuilder
    {
        private readonly IRequestContext _requestContext;

        public RestClientBuilder(IRequestContext requestContext)
        {
            _requestContext = requestContext ?? throw new ArgumentNullException(nameof(requestContext));
        }

        public T Build<T>(string baseUrl, Action<IConfigureRestClient> configure) where T : IRestClient
        {
            if (baseUrl == null) throw new ArgumentNullException(nameof(baseUrl));
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            var httpContext = new Uri(baseUrl).ToHttpContext(m => m.EnableDefaultCredential());
            var options = new RestClientOptions();

            configure(options);

            var client = RestClient.For<T>(httpContext);
            
            // TODO: Should be set into headers directly rather than property. It will be an issue with scoped singleton scope.
            client.CorrelationId = _requestContext.CorrelationId;

            if (options.IncludeAuthorizationHeader && !string.IsNullOrWhiteSpace(_requestContext.AuthorizationHeader))
                client.Authorization = AuthenticationHeaderValue.Parse(_requestContext.AuthorizationHeader);

            return client;
        }
        
        public T Build<T>(string baseUrl) where T : IRestClient
        {
            return Build<T>(baseUrl, m => { });
        }
    }
}
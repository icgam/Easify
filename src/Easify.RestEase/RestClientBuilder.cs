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
using EasyApi.Http;
using EasyApi.RestEase.Client;
using RestEase;

namespace Easify.RestEase
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
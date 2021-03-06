﻿// This software is part of the Easify framework
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
using Microsoft.AspNetCore.Mvc.Testing;
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
        
        public static T CreateClient<TStartup, T>(this WebApplicationFactory<TStartup> server) where T : IRestClient where TStartup : class
        {
            if (server == null) throw new ArgumentNullException(nameof(server));

            return RestClient.For<T>(server.CreateClient());
        }
    }
}
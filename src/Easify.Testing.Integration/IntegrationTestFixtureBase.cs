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
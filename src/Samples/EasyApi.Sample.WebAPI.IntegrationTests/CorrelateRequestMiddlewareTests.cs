// This software is part of the EasyApi framework
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
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EasyApi.AspNetCore.RequestCorrelation.Core;
using EasyApi.AspNetCore.RequestCorrelation.Core.OptionsBuilder;
using EasyApi.AspNetCore.RequestCorrelation.Domain;
using EasyApi.Http;
using EasyApi.Sample.WebAPI.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Xunit;

namespace EasyApi.Sample.WebAPI.IntegrationTests
{
    public class CorrelateRequestMiddlewareTests
    {
        private DefaultHttpContext GetDefaultHttpContext()
        {
            var context = new DefaultHttpContext();
            var responseFeature = Substitute.For<IHttpResponseFeature>();
            context.Features.Set(responseFeature);
            responseFeature.OnStarting(state => Task.FromResult(0), context);
            return context;
        }

        [Theory]
        [InlineData("X-Correlation-ID")]
        [InlineData("X-Request-ID")]
        public async Task Given_CorrelationId_IsPresent_ThenInvokeNextMiddlewareComponent(string header)
        {
            // Arrange
            var idProvider = Substitute.For<ICorrelationIdProvider>();
            var options = new RequestCorrelationOptions(true, new List<ICheckUrl>());
            var next = Substitute.For<RequestDelegate>();
            var sut = new CorrelateRequestMiddleware(next, options, idProvider);
            var context = GetDefaultHttpContext();
            context.Request.Headers.Add(header, Guid.NewGuid().ToString());

            // Act
            await sut.Invoke(context);

            // Assert
            await next.Received(1).Invoke(context);
        }

        [Fact]
        public async Task Given_CorrelationId_IsPresent_ThenResponseShouldHaveSameRequestId()
        {
            // Arrange
            var requestId = Guid.NewGuid().ToString();
            var builder = new WebHostBuilder();
            builder.UseStartup<CorrelatedRequestsStartup>();

            // Act
            HttpResponseMessage response;

            using (var server = new TestServer(builder))
            {
                using (var client = server.CreateClient())
                {
                    client.AddRequestIdToHeader(requestId);
                    response = await client.GetAsync("/");
                }
            }

            // Assert
            Assert.Collection(response.Headers, pair1 =>
            {
                Assert.Equal("X-Request-ID", pair1.Key);
                Assert.Equal(requestId, pair1.Value.Single());
            });
        }

        [Fact]
        public async Task Given_CorrelationId_NotPresent_ShouldThrow()
        {
            // Arrange
            var idProvider = Substitute.For<ICorrelationIdProvider>();
            var options = new RequestCorrelationOptions(true, new List<ICheckUrl>());
            var next = Substitute.For<RequestDelegate>();
            var sut = new CorrelateRequestMiddleware(next, options, idProvider);
            var context = GetDefaultHttpContext();

            // Act
            // Assert
            await Assert.ThrowsAsync<NotCorrelatedRequestException>(() => sut.Invoke(context));
        }

        [Fact]
        public async Task
            Given_CorrelationId_NotPresent_WhenOptionsAllowBypassingUrl_IfUrlContainsGivenFragment_ThenResponseShouldReturn_200OK()
        {
            // Arrange
            var optionsBuilder = new CorrelationOptionsBuilder();
            var options = optionsBuilder.ExcludeUrl(f => f.WhenContains("nostics/lo")).EnforceCorrelation().Build();
            var builder = new WebHostBuilder();
            builder.UseStartup<CorrelatedRequestsStartup>();
            builder.ConfigureServices(services => { services.Replace(ServiceDescriptor.Singleton(c => options)); });

            // Act
            HttpResponseMessage response;

            using (var server = new TestServer(builder))
            {
                using (var client = server.CreateClient())
                {
                    response = await client.GetAsync("/diagnostics/logs");
                }
            }

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task
            Given_CorrelationId_NotPresent_WhenOptionsAllowBypassingUrl_IfUrlEndsAsSpecified_ThenResponseShouldReturn_200OK()
        {
            // Arrange
            var optionsBuilder = new CorrelationOptionsBuilder();
            var options = optionsBuilder.ExcludeUrl(f => f.WhenEndsWith("/logs")).EnforceCorrelation().Build();
            var builder = new WebHostBuilder();
            builder.UseStartup<CorrelatedRequestsStartup>();
            builder.ConfigureServices(services => { services.Replace(ServiceDescriptor.Singleton(c => options)); });

            // Act
            HttpResponseMessage response;

            using (var server = new TestServer(builder))
            {
                using (var client = server.CreateClient())
                {
                    response = await client.GetAsync("/diagnostics/logs");
                }
            }

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task
            Given_CorrelationId_NotPresent_WhenOptionsAllowBypassingUrl_IfUrlStartsAsSpecified_ThenResponseShouldReturn_200OK()
        {
            // Arrange
            var optionsBuilder = new CorrelationOptionsBuilder();
            var options = optionsBuilder.ExcludeUrl(f => f.WhenStartsWith("/diagnostics/")).EnforceCorrelation()
                .Build();
            var builder = new WebHostBuilder();
            builder.UseStartup<CorrelatedRequestsStartup>();
            builder.ConfigureServices(services => { services.Replace(ServiceDescriptor.Singleton(c => options)); });

            // Act
            HttpResponseMessage response;

            using (var server = new TestServer(builder))
            {
                using (var client = server.CreateClient())
                {
                    response = await client.GetAsync("/diagnostics/logs");
                }
            }

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task
            Given_CorrelationId_NotPresent_WhenOptionsNotEnforcingMessageCorrelation_ThenRequestShouldContainGeneratedId()
        {
            // Arrange
            var idProvider = Substitute.For<ICorrelationIdProvider>();
            var options = new RequestCorrelationOptions(false, new List<ICheckUrl>());
            var next = Substitute.For<RequestDelegate>();
            var sut = new CorrelateRequestMiddleware(next, options, idProvider);
            var context = GetDefaultHttpContext();
            idProvider.GenerateId().Returns("my-id");

            // Act
            await sut.Invoke(context);

            // Assert
            await next.Received(1).Invoke(Arg.Is<HttpContext>(c => c.Request.Headers.Values.Contains("my-id")));
        }

        [Fact]
        public async Task
            Given_CorrelationId_NotPresent_WhenOptionsNotEnforcingMessageCorrelation_ThenResponseShouldContainGeneratedId()
        {
            // Arrange
            var idProvider = Substitute.For<ICorrelationIdProvider>();
            var builder = new WebHostBuilder();

            idProvider.GenerateId().Returns("my-id");

            builder.UseStartup<CorrelatedRequestsStartup>();
            builder.ConfigureServices(services =>
            {
                services.Replace(ServiceDescriptor.Singleton(c =>
                    new RequestCorrelationOptions(false, new List<ICheckUrl>())));
                services.Replace(ServiceDescriptor.Singleton(c => idProvider));
            });

            // Act
            HttpResponseMessage response;

            using (var server = new TestServer(builder))
            {
                using (var client = server.CreateClient())
                {
                    response = await client.GetAsync("/");
                }
            }

            // Assert
            Assert.Collection(response.Headers, pair1 =>
            {
                Assert.Equal("X-Request-ID", pair1.Key);
                Assert.Equal("my-id", pair1.Value.Single());
            });
        }

        [Fact]
        public void Given_NoCorrelationIdProviderSupplied_ShouldThrow()
        {
            // Arrange
            var options = new RequestCorrelationOptions(false, new List<ICheckUrl>());
            var next = Substitute.For<RequestDelegate>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new CorrelateRequestMiddleware(next, options, null));
        }

        [Fact]
        public void Given_NoOptionsSupplied_ShouldThrow()
        {
            // Arrange
            var idProvider = Substitute.For<ICorrelationIdProvider>();
            var next = Substitute.For<RequestDelegate>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new CorrelateRequestMiddleware(next, null, idProvider));
        }
    }
}
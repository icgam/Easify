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

using System.Linq;
using Easify.DependencyInjection;
using EasyApi.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EasyApi.AspNetCore.Bootstrap.UnitTests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void Should_RemoveFirstOrNothing_InServiceCollection_DoesNothingWhenTheServiceIsNotRegistered()
        {
            // Arrange
            var services = new ServiceCollection();
            services.RemoveFirstOrNothing<IRequestContext>();

            // Act
            var requestcontextList = services.BuildServiceProvider().GetServices<IRequestContext>();

            // Assert
            Assert.NotNull(requestcontextList);
            Assert.Empty(requestcontextList);
        }


        [Fact]
        public void
            Should_RemoveFirstOrNothing_InServiceCollection_RemoveTheServiceCorrectlyWhenTheServiceIsRegistered()
        {
            // Arrange
            var services = new ServiceCollection();
            services.RemoveFirstOrNothing<IRequestContext>();

            // Act
            var requestcontextList = services.BuildServiceProvider().GetServices<IRequestContext>();

            // Assert
            Assert.NotNull(requestcontextList);
            Assert.Empty(requestcontextList);
        }

        [Fact]
        public void Should_ReplaceFirst_InServiceCollection_AddTheServiceCorrectlyWhenTheServiceIsNotRegistered()
        {
            // Arrange
            var services = new ServiceCollection();
            services.ReplaceFirst<IRequestContext, DummyRequestContext>(ServiceLifetime.Transient);

            // Act
            var requestcontextList = services.BuildServiceProvider().GetServices<IRequestContext>();

            // Assert
            Assert.NotNull(requestcontextList);

            var requestContext = requestcontextList.First() as DummyRequestContext;
            Assert.IsType<DummyRequestContext>(requestContext);
        }

        [Fact]
        public void
            Should_ReplaceFirst_InServiceCollection_ReplaceTheServiceCorrectlyWhenTheServiceIsAlreadyRegistered()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddHttpRequestContext();
            services.ReplaceFirst<IRequestContext, DummyRequestContext>(ServiceLifetime.Transient);

            // Act
            var requestcontextList = services.BuildServiceProvider().GetServices<IRequestContext>();

            // Assert
            Assert.NotNull(requestcontextList);

            var requestContext = requestcontextList.First() as DummyRequestContext;
            Assert.IsType<DummyRequestContext>(requestContext);
        }
    }
}
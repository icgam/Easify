using System.Linq;
using EasyApi.DependencyInjection;
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
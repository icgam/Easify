using System.Threading.Tasks;
using EasyApi.Sample.WebAPI.Core;
using EasyApi.Sample.WebAPI.IntegrationTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Xunit;

namespace EasyApi.Sample.WebAPI.IntegrationTests
{
    public sealed class StartupServiceCollectionSetupTests
    {
        [Fact]
        public async Task GivenApiWhenNoServiceOverridedThenReturnResultAsExpected()
        {
            // Arrange
            const string dataToProcess = "my-data";

            // Act
            using (var fixture = TestServerFixture<StartupForIntegration>.Create())
            {
                var response = await fixture.Client.GetAsync($"api/Service/{dataToProcess}");
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.Equal($"Processed {dataToProcess}", responseString);
            }
        }

        [Fact]
        public async Task GivenApiWhenServiceIsOverridedThenReturnResultFromFakeService()
        {
            // Arrange
            const string dataToProcess = "my-data";

            var myServiceMock = Substitute.For<IMyService>();
            myServiceMock.Process(Arg.Any<string>())
                .Returns(ci => $"Processed {dataToProcess} using very FAKE service");

            // Act
            using (var fixture = TestServerFixture<StartupForIntegration>.Create(s =>
            {
                s.Replace(ServiceDescriptor.Transient(c => myServiceMock));
            }))
            {
                var response = await fixture.Client.GetAsync($"api/Service/{dataToProcess}");
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.Equal($"Processed {dataToProcess} using very FAKE service", responseString);
            }
        }
    }
}
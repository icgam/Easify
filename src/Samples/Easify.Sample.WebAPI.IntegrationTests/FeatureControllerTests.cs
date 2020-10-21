using System.Net;
using System.Threading.Tasks;
using Easify.Sample.WebAPI.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace Easify.Sample.WebAPI.IntegrationTests
{
    public class FeatureControllerTests
    {
        [Theory]
        [InlineData("Feature1", HttpStatusCode.OK, true)]
        [InlineData("Feature2", HttpStatusCode.OK, false)]
        [InlineData("Feature3", HttpStatusCode.NotFound, false)]
        public async Task GivenAFeature_WhenRequestedFeatureStateIsPresentedInConfiguration_ThenReturnTheStatus(string name, HttpStatusCode statusCode, bool expected)
        {
            // Arrange
            using var fixture = TestApplicationFactory<StartupForIntegration>.Create();
            
            // Act
            var response = await fixture.CreateClient().GetAsync($"api/feature/{name}");

            // Assert
            response.StatusCode.Should().Be(statusCode);
            var actual = await response.Content.ReadAsStringAsync();
            
            actual.Should().Be(expected.ToString().ToLower());
        }
    }
}
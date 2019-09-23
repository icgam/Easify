using System;
using System.Net;
using System.Threading.Tasks;
using EasyApi.Sample.WebAPI.Domain;
using EasyApi.Sample.WebAPI.IntegrationTests.Helpers;
using Newtonsoft.Json;
using Xunit;

namespace EasyApi.Sample.WebAPI.IntegrationTests
{
    public sealed class AutomapperControllerTests : IDisposable
    {
        public AutomapperControllerTests()
        {
            Fixture = TestServerFixture<StartupForAutomapper>.Create();
        }

        private TestServerFixture<StartupForAutomapper> Fixture { get; }

        public void Dispose()
        {
            try
            {
                Fixture.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Theory]
        [InlineData("John", "Dow")]
        [InlineData("Jane", "Dow")]
        public async Task GivenUserWhenRequestedShouldMapAndReturnCorrectUserResult(string firstName, string lastName)
        {
            // Arrange
            // Act
            var response = await Fixture.Client.GetAsync($"api/automapper/person/{firstName}/{lastName}");
            var responseString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<PersonDO>(responseString);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(firstName, content.FirstName);
            Assert.Equal(lastName, content.LastName);
        }

        [Theory]
        [InlineData("ast1")]
        [InlineData("ast2")]
        [InlineData("ast3")]
        public async Task GivenAssetWhenRequestedShouldMapAndReturnAssetWithCorrectRating(string assetId)
        {
            // Arrange
            // Act
            var response = await Fixture.Client.GetAsync($"api/automapper/asset/{assetId}");
            var responseString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<AssetDO>(responseString);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(assetId, content.Id);
            Assert.Equal("AAA", content.Rating);
        }
    }
}
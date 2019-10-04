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
using System.Net;
using System.Threading.Tasks;
using Easify.Sample.WebAPI.Domain;
using Easify.Sample.WebAPI.IntegrationTests.Helpers;
using Newtonsoft.Json;
using Xunit;

namespace Easify.Sample.WebAPI.IntegrationTests
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
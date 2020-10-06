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
using Easify.Sample.WebAPI.IntegrationTests.Helpers;
using Easify.Sample.WebAPI.IntegrationTests.Helpers.Domain;
using Newtonsoft.Json;
using Xunit;

namespace Easify.Sample.WebAPI.IntegrationTests
{
    public sealed class ErrorsProducingControllerTests : IDisposable
    {
        public ErrorsProducingControllerTests()
        {
            Fixture = TestServerFixture<StartupForIntegration>.Create();
        }

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

        private TestServerFixture<StartupForIntegration> Fixture { get; }

        [Fact]
        public async Task ShouldThrowFriendlyException()
        {
            // Arrange
            // Act
            var response = await Fixture.Client.GetAsync("api/Errors/throwFriendly");
            var responseString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ErrorResponse>(responseString);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("I am a user friendly exception message", content.Message);
            Assert.Collection(content.UserErrors, e1 =>
            {
                Assert.Equal("I am a user friendly exception message", e1.Message);
                Assert.Equal("OurApplicationException", e1.ErrorType);
                Assert.Empty(e1.ChildErrors);
            });
            Assert.Empty(content.RawErrors);
        }

        [Fact]
        public async Task ShouldThrowFriendlyHierarchyOfExceptions()
        {
            // Arrange
            // Act
            var response = await Fixture.Client.GetAsync("api/Errors/throwHierarchy");
            var responseString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ErrorResponse>(responseString);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Admin root thrown!", content.Message);
            Assert.Collection(content.UserErrors, e1 =>
            {
                Assert.Equal("Admin root thrown!", e1.Message);
                Assert.Equal("OurApplicationException", e1.ErrorType);
                Assert.Collection(e1.ChildErrors, t1 =>
                    {
                        Assert.Equal("My friendly leaf1!", t1.Message);
                        Assert.Equal("OurApplicationException", t1.ErrorType);
                        Assert.Empty(t1.ChildErrors);
                    },
                    t2 =>
                    {
                        Assert.Equal("Third party plugin has failed!", t2.Message);
                        Assert.Equal("ThirdPartyPluginFailedException", t2.ErrorType);
                        Assert.Empty(t2.ChildErrors);
                    }
                );
            });
            Assert.Empty(content.RawErrors);
        }

        [Fact]
        public async Task ShouldThrowThirdPartyException()
        {
            // Arrange
            // Act
            var response = await Fixture.Client.GetAsync("api/Errors/throwThirdParty");
            var responseString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ErrorResponse>(responseString);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Third party plugin has failed!", content.Message);
            Assert.Collection(content.UserErrors, e1 =>
            {
                Assert.Equal("Third party plugin has failed!", e1.Message);
                Assert.Equal("ThirdPartyPluginFailedException", e1.ErrorType);
                Assert.Empty(e1.ChildErrors);
            });
            Assert.Empty(content.RawErrors);
        }

        [Fact]
        public async Task ShouldThrowUnFriendlyException()
        {
            // Arrange
            // Act
            var response = await Fixture.Client.GetAsync("api/Errors/throwUnfriendly");
            var responseString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ErrorResponse>(responseString);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Equal("Unknown Error Happened", content.Message);
            Assert.Collection(content.UserErrors, e1 =>
            {
                Assert.Equal("Unknown Error Happened", e1.Message);
                Assert.Equal("UnknownException", e1.ErrorType);
                Assert.Empty(e1.ChildErrors);
            });
            Assert.Empty(content.RawErrors);
        }
    }
}
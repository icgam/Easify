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
using Xunit;

namespace Easify.Sample.WebAPI.IntegrationTests
{
    public sealed class ConfigurationControllerTests
    {
        public ConfigurationControllerTests()
        {
            Environment.SetEnvironmentVariable("EASIFY_EnvVar1", "SampleValue1");
            Environment.SetEnvironmentVariable("EASIFY_EnvVar3", "SampleValue3");
        }

        [Theory]
        [InlineData("EnvVar1", HttpStatusCode.OK, "SampleValue1")]
        [InlineData("EnvVar2", HttpStatusCode.OK, "OriginalValue")]
        [InlineData("EnvVar3", HttpStatusCode.OK, "SampleValue3")]
        [InlineData("EnvVar4", HttpStatusCode.NotFound, "")]
        public async Task GivenEnvironmentVariableWhenIsNotAvailableItShouldReturnRelevantHttpResponse(
            string key, HttpStatusCode expectedStatus, string expectedValue)
        {
            // Arrange
            using var fixture = TestApplicationFactory<StartupForConfiguration>.Create();

            // Act
            var response = await fixture.Client.GetAsync($"api/configuration/environment/{key}");
            var responseString = await response.Content.ReadAsStringAsync();
            
            // Assert
            Assert.Equal(expectedStatus, response.StatusCode);
            if (expectedStatus == HttpStatusCode.OK)
                Assert.Equal(expectedValue, responseString);
        }        
    }
}
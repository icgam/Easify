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

using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Easify.AspNetCore.Security.Impersonation;
using Easify.Sample.WebAPI.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace Easify.Sample.WebAPI.IntegrationTests
{
    public sealed class AuthenticationControllerTests
    {
        private const string DefaultImpersonationToken =
            "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJpZGVudGlmaWVyIjoiMSIsImdpdmVuX25hbWUiOiJTaW1vbiIsImZhbWlseV9uYW1lIjoiUGVyZXoiLCJuYW1lIjoiU2ltb24gUGVyZXoiLCJlbWFpbCI6InNpbW9uLnBlcmV6QGljZ2FtLmNvbSIsInByZWZlcnJlZF91c2VybmFtZSI6InNpbW9uLnBlcmV6QGljZ2FtLmNvbSIsInVuaXF1ZV9uYW1lIjoic2ltb24ucGVyZXpAaWNnYW0uY29tIiwicm9sZXMiOlsiUy1NRVpaQU5JTkUtUFJPRC1NREEiXSwiZ3JvdXBzIjpbIlMtTUVaWkFOSU5FLVBST0QtTURBIl0sImV4cCI6MTU4Nzc3MjgwMH0.Fq5iIe1McVjNuju2K73qiAqiVsV0kk2QCRybnVoQybk";
        
        [Fact]
        public async Task GivenAuthenticationWithOAuth2_WhenQueryingTheSecureResourceWithoutAuthorizationHeader_ThenGetInternalServerError()
        {
            // Arrange
            using var fixture = TestApplicationFactory<StartupForNoAuth>.Create();
            
            // Act
            var response = await fixture.Client.GetAsync("api/Authentication/secured");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }              
        
        [Fact]
        public async Task GivenNoAuthentication_WhenQueryingTheSecureResource_ThenGetUnauthorizedStatus()
        {
            // Arrange
            using var fixture = TestApplicationFactory<StartupForOAuth2>.Create();
            
            // Act
            var response = await fixture.Client.GetAsync("api/Authentication/secured");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }        
        
        [Fact]
        public async Task GivenAuthenticationWithOAuth2_WhenQueryingTheUnsecureResourceWithoutAuthorizationHeader_ThenGetOkStatus()
        {
            // Arrange
            using var fixture = TestApplicationFactory<StartupForOAuth2>.Create(); 
            
            // Act
            var response = await fixture.Client.GetAsync("api/Authentication/unsecured");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task GivenAuthenticationWithImpersonation_WhenQueryingTheResourceWithoutAuthorizationHeader_ThenGetUnauthorizedStatus()
        {
            // Arrange
            using var fixture = TestApplicationFactory<StartupForImpersonation>.Create();
            
            // Act
            var response = await fixture.Client.GetAsync("api/Authentication/secured");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }     
        
        [Theory]
        [InlineData("Invalid Header", HttpStatusCode.Unauthorized)]
        [InlineData("InvalidHeader", HttpStatusCode.Unauthorized)]
        [InlineData(DefaultImpersonationToken, HttpStatusCode.OK)]
        public async Task GivenAuthenticationWithImpersonation_WhenQueryingTheResourceAuthorizationHeader_ThenGetRelevantStatus(string header, HttpStatusCode statusCode)
        {

            
            // Arrange
            using var fixture = TestApplicationFactory<StartupForImpersonation>.Create();
            fixture.Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(ImpersonationBearerDefaults.AuthenticationScheme, header);
            // Act
            var response = await fixture.Client.GetAsync("api/Authentication/secured");

            // Assert
            response.StatusCode.Should().Be(statusCode);
        }
    }
}
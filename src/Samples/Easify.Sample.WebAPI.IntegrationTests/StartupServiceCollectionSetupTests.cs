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

using System.Threading.Tasks;
using Easify.Sample.WebAPI.Core;
using Easify.Sample.WebAPI.IntegrationTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Xunit;

namespace Easify.Sample.WebAPI.IntegrationTests
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
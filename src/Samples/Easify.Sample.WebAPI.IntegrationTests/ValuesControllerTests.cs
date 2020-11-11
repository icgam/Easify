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
using System.Threading.Tasks;
using Easify.Sample.WebAPI.Core;
using Easify.Sample.WebAPI.IntegrationTests.Helpers;
using Easify.Testing.Integration;
using FluentAssertions;
using Xunit;

namespace Easify.Sample.WebAPI.IntegrationTests
{
    public sealed class ValuesControllerTests : IClassFixture<IntegrationTestFixture<StartupForValues>>
    {
        private readonly IntegrationTestFixture<StartupForValues> _fixture;

        public ValuesControllerTests(IntegrationTestFixture<StartupForValues> fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public async Task ValuesGetShouldReturnTheValuesCorrectly()
        {
            // Arrange
            using var pair = _fixture.CreateClientFromServer<IValuesClient>(s => { });

            // Act
            var actual = (await pair.Client.GetValuesAsync()).ToArray();

            // Assert
            actual.Should().NotBeNull();
            actual.Should().HaveCount(2)
                .And.Contain(m => m == "value1")
                .And.Contain(m => m == "value2");
        }
    }
}
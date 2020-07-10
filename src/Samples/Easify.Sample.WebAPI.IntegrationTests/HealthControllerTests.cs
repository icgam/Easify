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
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Easify.Sample.WebAPI.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Xunit;

namespace Easify.Sample.WebAPI.IntegrationTests
{
    public sealed class HealthControllerTests
    {
        [Fact]
        public async Task GivenAPIRunning_WhenHealthRequested_ShouldReturnHealthy()
        {
            // Arrange
            using (var fixture = TestServerFixture<StartupForHealthy>.Create())
            {
                // Act
                var response = await fixture.Client.GetAsync($"health");
                var responseString = await response.Content.ReadAsStringAsync();
                var actual = JsonConvert.DeserializeObject<HealthData>(responseString);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                actual.Status.Should().Be(HealthStatus.Healthy);
                actual.Entries["microsoft"].Status.Should().Be(HealthStatus.Healthy);
                actual.Entries["microsoft"].Exception.Should().BeNull();
            }
        }        
        
        [Fact]
        public async Task GivenAPIRunning_WhenHealthRequested_ShouldReturnUnhealthy()
        {
            // Arrange
            using (var fixture = TestServerFixture<StartupForUnhealthy>.Create())
            {
                // Act
                var response = await fixture.Client.GetAsync($"health");
                var responseString = await response.Content.ReadAsStringAsync();
                var actual = JsonConvert.DeserializeObject<HealthData>(responseString);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
                actual.Status.Should().Be(HealthStatus.Unhealthy);
                actual.Entries["microsoft"].Status.Should().Be(HealthStatus.Healthy);
                actual.Entries["microsoft"].Exception.Should().BeNull();
                actual.Entries["microsoftwithwrongurl"].Status.Should().Be(HealthStatus.Unhealthy);
                actual.Entries["microsoftwithwrongurl"].Exception.Should().NotBeNull();
                actual.Entries["microsoftwithwrongurl"].Description.Should().NotBeNull();
            }
        }

        private class HealthData
        {
            public Dictionary<string, HealthDataEntry> Entries { get; set; }
            public HealthStatus Status { get; set; }
            public TimeSpan TotalDuration { get; set; }
        }
        
        private class HealthDataEntry
        {
            public string Description { get; set; }
            public TimeSpan Duration { get; set; }
            public string Exception { get; set; }
            public HealthStatus Status { get; set; }
        }
    }
}
// This software is part of the EasyApi framework
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

﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EasyApi.Sample.WebAPI.Domain;
using EasyApi.Sample.WebAPI.IntegrationTests.Helpers;
using Xunit;

namespace EasyApi.Sample.WebAPI.IntegrationTests
{
    public sealed class ValidationControllerTests : IDisposable
    {
        public ValidationControllerTests()
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

        private const int UnprocessableEntity = 422;

        private TestServerFixture<StartupForIntegration> Fixture { get; }

        [Theory]
        [InlineData("Validate")]
        [InlineData("Process")]
        [InlineData("Publish")]
        public async Task ForPostShouldReturn200Ok(string operation)
        {
            // Arrange
            var request = new StoreDocumentsRequest
            {
                RequestId = Guid.NewGuid(),
                Operation = operation,
                Owner = new Owner(),
                Documents = new List<Document>
                {
                    new Document()
                }
            };

            // Act
            var response = await Fixture.Client.PostAsync("api/Validation", new JsonContent(request));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ForPostShouldReturnErrorWithMissingArguments()
        {
            // Arrange
            // Act
            var response = await Fixture.Client.PostAsync("api/Validation",
                new StringContent(string.Empty, Encoding.UTF8, "application/json"));

            // Assert
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(UnprocessableEntity, (int) response.StatusCode);
            Assert.Contains("A non-empty request body is required", responseString);
        }

        [Fact]
        public async Task ForPostShouldReturnWithModelValidationError()
        {
            // Arrange
            var request = new StoreDocumentsRequest
            {
                RequestId = Guid.NewGuid(),
                Operation = "INVALID",
                Owner = new Owner(),
                Documents = new List<Document>()
            };

            // Act
            var response = await Fixture.Client.PostAsync("api/Validation", new JsonContent(request));

            // Assert
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(UnprocessableEntity, (int) response.StatusCode);
            Assert.NotNull(response);
            Assert.Contains(
                $"'{request.Operation}' is not supported. Supported operations are: Validate, Process, Publish.",
                responseString);
        }

        [Fact]
        public async Task ForPostShouldReturnWithMultipleModelValidationErrors()
        {
            // Arrange
            var request = new StoreDocumentsRequest
            {
                Documents = new List<Document>
                {
                    new Document()
                }
            };

            // Act
            var response = await Fixture.Client.PostAsync("api/Validation", new JsonContent(request));

            // Assert
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(UnprocessableEntity, (int) response.StatusCode);
            Assert.Contains("Operation must be supplied! Supported operations are: Validate, Process, Publish.",
                responseString);
            Assert.Contains("'Request Id' should not be empty.", responseString);
        }
    }
}
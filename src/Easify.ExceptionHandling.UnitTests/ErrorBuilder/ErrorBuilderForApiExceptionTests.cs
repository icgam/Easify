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
using System.Net.Http;
using EasyApi.ExceptionHandling.Domain;
using EasyApi.ExceptionHandling.ErrorBuilder;
using RestEase;
using Xunit;

namespace Easify.ExceptionHandling.UnitTests.ErrorBuilder
{
    public sealed class ErrorBuilderForApiExceptionTests
    {
        private ApiException BuildApiException(string content = "")
        {
            return new ApiException(HttpMethod.Get, new Uri("api/health", UriKind.Relative), HttpStatusCode.BadRequest,
                "reason", null, null, content);
        }

        [Theory]
        [InlineData("{\"Messag\":,\"UserErrors\":,\"RawErrors\":}")]
        [InlineData("{\"Message\":,\"UserError\":,\"RawErrors\":}")]
        [InlineData("{\"Message\":,\"UserErrors\":,\"RawError\":}")]
        public void GivenExceptionWhenContentDoesntHaveRequiredKeywordsThenErrorWithContentShouldBeReturned(
            string content)
        {
            // Arrange
            var sut = new ErrorBuilderForApiException();
            var exception = BuildApiException(content);
            var internalErrors = new List<Error>();

            // Act
            var result = sut.Build(exception, internalErrors, false);

            // Assert
            Assert.Equal(
                "GET \"api/health\" failed because response status code does not indicate success: 400 (reason).",
                result.Message);
            Assert.Equal("RestEase.ApiException", result.ErrorType);
            Assert.Collection(result.ChildErrors,
                e =>
                {
                    Assert.Equal(content, e.Message);
                    Assert.Equal("RestEase.ApiException", e.ErrorType);
                    Assert.Same(internalErrors, e.ChildErrors);
                });
        }

        [Fact]
        public void GivenAnyExceptionWhenExceptionIsNotSuppliedThenBuilderShouldThrow()
        {
            // Arrange
            var sut = new ErrorBuilderForApiException();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => sut.Build(null, new List<Error>(), false));
        }

        [Fact]
        public void GivenAnyExceptionWhenInternalErrorsAreNotSuppliedThenBuilderShouldThrow()
        {
            // Arrange
            var sut = new ErrorBuilderForApiException();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => sut.Build(
                    BuildApiException(), null, false));
        }

        [Fact]
        public void GivenExceptionWhenContentHasMalformedErrorThenReturnErrorWithContentShouldBeReturned()
        {
            // Arrange
            var sut = new ErrorBuilderForApiException();
            var exception = BuildApiException("{\"Message\":\"My Error!\",\"UserErrors\":[XXXXXX],\"RawErrors\":[]}");
            var internalErrors = new List<Error>();

            // Act
            var result = sut.Build(exception, internalErrors, false);

            // Assert
            Assert.Equal(
                "GET \"api/health\" failed because response status code does not indicate success: 400 (reason).",
                result.Message);
            Assert.Equal("RestEase.ApiException", result.ErrorType);
            Assert.Collection(result.ChildErrors,
                e =>
                {
                    Assert.Equal("{\"Message\":\"My Error!\",\"UserErrors\":[XXXXXX],\"RawErrors\":[]}", e.Message);
                    Assert.Equal("RestEase.ApiException", e.ErrorType);
                    Assert.Same(internalErrors, e.ChildErrors);
                });
        }

        [Fact]
        public void
            GivenExceptionWhenContentHasSerializedErrorAndLevelOfDetailsIsDetailedErrorsButNoErrorsReturnedThenUseUserErrorsInstead()
        {
            // Arrange
            var content =
                "{\"Message\":\"My Error!\"," +
                "\"UserErrors\":[ { \"Message\":\"UserMessage\", \"ErrorType\":\"ExternalUserError\", \"ChildErrors\":[] } ]," +
                "\"RawErrors\":[]}";

            var sut = new ErrorBuilderForApiException();
            var exception = BuildApiException(content);

            // Act
            var result = sut.Build(exception, new List<Error>
            {
                new Error("InternalError", "Exception")
            }, true);

            // Assert
            Assert.Equal(
                "GET \"api/health\" failed because response status code does not indicate success: 400 (reason).",
                result.Message);
            Assert.Equal("RestEase.ApiException", result.ErrorType);
            Assert.Collection(result.ChildErrors,
                ep =>
                {
                    Assert.Collection(ep.ChildErrors,
                        e =>
                        {
                            Assert.Equal("InternalError", e.Message);
                            Assert.Equal("Exception", e.ErrorType);
                        }, e =>
                        {
                            Assert.Equal("UserMessage", e.Message);
                            Assert.Equal("ExternalUserError", e.ErrorType);
                        });
                });
        }

        [Fact]
        public void
            GivenExceptionWhenContentHasSerializedErrorAndLevelOfDetailsIsDetailedErrorsThenReturnInteralErrorsAndDetailedErrors()
        {
            // Arrange
            var content =
                "{\"Message\":\"My Error!\"," +
                "\"UserErrors\":[ { \"Message\":\"UserMessage\", \"ErrorType\":\"ExternalUserError\", \"ChildErrors\":[] } ]," +
                "\"RawErrors\":[ { \"Message\":\"RawMessage\", \"ErrorType\":\"ExternalRawError\", \"ChildErrors\":[] } ]}";

            var sut = new ErrorBuilderForApiException();
            var exception = BuildApiException(content);

            // Act
            var result = sut.Build(exception, new List<Error>
            {
                new Error("InternalError", "Exception")
            }, true);

            // Assert
            Assert.Equal(
                "GET \"api/health\" failed because response status code does not indicate success: 400 (reason).",
                result.Message);
            Assert.Equal("RestEase.ApiException", result.ErrorType);
            Assert.Collection(result.ChildErrors,
                ep =>
                {
                    Assert.Collection(ep.ChildErrors,
                        e =>
                        {
                            Assert.Equal("InternalError", e.Message);
                            Assert.Equal("Exception", e.ErrorType);
                        }, e =>
                        {
                            Assert.Equal("RawMessage", e.Message);
                            Assert.Equal("ExternalRawError", e.ErrorType);
                        });
                });
        }

        [Fact]
        public void GivenExceptionWhenContentHasSerializedErrorAndLevelOfDetailsIsStandardThenReturnInteralErrorsOnly()
        {
            // Arrange
            var sut = new ErrorBuilderForApiException();
            var exception = BuildApiException("{\"Message\":\"My Error!\",\"UserErrors\":[],\"RawErrors\":[]}");

            // Act
            var result = sut.Build(exception, new List<Error>
            {
                new Error("InternalError", "Exception")
            }, false);

            // Assert
            Assert.Equal(
                "GET \"api/health\" failed because response status code does not indicate success: 400 (reason).",
                result.Message);
            Assert.Equal("RestEase.ApiException", result.ErrorType);
            Assert.Collection(result.ChildErrors,
                ep =>
                {
                    Assert.Collection(ep.ChildErrors,
                        e =>
                        {
                            Assert.Equal("InternalError", e.Message);
                            Assert.Equal("Exception", e.ErrorType);
                        });
                });
        }

        [Fact]
        public void
            GivenExceptionWhenContentHasSerializedErrorAndLevelOfDetailsIsUserErrorsThenReturnInteralErrorsAndUserErrors()
        {
            // Arrange
            var content =
                "{\"Message\":\"My Error!\"," +
                "\"UserErrors\":[ { \"Message\":\"UserMessage\", \"ErrorType\":\"ExternalUserError\", \"ChildErrors\":[] } ]," +
                "\"RawErrors\":[ { \"Message\":\"RawMessage\", \"ErrorType\":\"ExternalRawError\", \"ChildErrors\":[] } ]}";

            var sut = new ErrorBuilderForApiException();
            var exception = BuildApiException(content);

            // Act
            var result = sut.Build(exception, new List<Error>
            {
                new Error("InternalError", "Exception")
            }, false);

            // Assert
            Assert.Equal(
                "GET \"api/health\" failed because response status code does not indicate success: 400 (reason).",
                result.Message);
            Assert.Equal("RestEase.ApiException", result.ErrorType);
            Assert.Collection(result.ChildErrors,
                ep =>
                {
                    Assert.Collection(ep.ChildErrors,
                        e =>
                        {
                            Assert.Equal("InternalError", e.Message);
                            Assert.Equal("Exception", e.ErrorType);
                        }, e =>
                        {
                            Assert.Equal("UserMessage", e.Message);
                            Assert.Equal("ExternalUserError", e.ErrorType);
                        });
                });
        }

        [Fact]
        public void GivenExceptionWhenContentHasSerializedErrorThenReturnOriginalError()
        {
            // Arrange
            var sut = new ErrorBuilderForApiException();
            var exception = BuildApiException("{\"Message\":\"My Error!\",\"UserErrors\":[],\"RawErrors\":[]}");

            // Act
            var result = sut.Build(exception, new List<Error>(), false);

            // Assert
            Assert.Equal(
                "GET \"api/health\" failed because response status code does not indicate success: 400 (reason).",
                result.Message);
            Assert.Equal("RestEase.ApiException", result.ErrorType);
            Assert.Collection(result.ChildErrors,
                ep =>
                {
                    Assert.Equal("My Error!", ep.Message);
                    Assert.Equal("RestEase.ApiException", ep.ErrorType);
                });
        }

        [Fact]
        public void GivenExceptionWhenNoContentFoundThenErrorWithMessageShouldBeReturned()
        {
            // Arrange
            var sut = new ErrorBuilderForApiException();
            var exception = BuildApiException();
            var internalErrors = new List<Error>();

            // Act
            var result = sut.Build(exception, internalErrors, false);

            // Assert
            Assert.Equal(
                "GET \"api/health\" failed because response status code does not indicate success: 400 (reason).",
                result.Message);
            Assert.Equal("RestEase.ApiException", result.ErrorType);
            Assert.Same(internalErrors, result.ChildErrors);
        }
    }
}
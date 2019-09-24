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
using System.Net;
using EasyApi.ExceptionHandling.Domain;
using EasyApi.ExceptionHandling.Formatter;
using EasyApi.ExceptionHandling.Providers;
using NSubstitute;
using Xunit;

namespace EasyApi.ExceptionHandling.UnitTests
{
    public sealed class ErrorResponseProviderTests
    {
        [Fact]
        public void GivenExceptionWhenLevelOfDetailsIsIncludeAllErrorsThenResponseShouldContainDetailedErrors()
        {
            // Arrange
            var error = new Error("Error", "Exception");
            var systemError = new Error("SystemError", "Exception");
            var exception = new Exception();
            var optionsMock = Substitute.For<IErrorResponseProviderOptions>();
            var errorProviderMock = Substitute.For<IErrorProvider>();
            var responseCodeProviderMock = Substitute.For<IHttpStatusCodeProvider>();

            optionsMock.ErrorLevelOfDetails.Returns(LevelOfDetails.DetailedErrors);
            errorProviderMock.ExtractErrorsFor(exception,
                    Arg.Is<IErrorProviderOptions>(o => o.IncludeSystemLevelExceptions == false))
                .Returns(error);
            errorProviderMock.ExtractErrorsFor(exception,
                    Arg.Is<IErrorProviderOptions>(o => o.IncludeSystemLevelExceptions))
                .Returns(systemError);

            var errorFormatterMock = Substitute.For<IErrorMessageFormatter>();
            errorFormatterMock.FormatErrorMessages(error)
                .Returns("Formatted Error");

            responseCodeProviderMock.GetHttpStatusCode(Arg.Any<Exception>())
                .Returns(HttpStatusCode.BadRequest);

            var sut = new ErrorResponseProvider(errorProviderMock, errorFormatterMock, responseCodeProviderMock,
                optionsMock);

            // Act
            var result = sut.GetErrors(exception);

            // Assert
            Assert.Equal("Formatted Error", result.Message);
            Assert.Collection(result.UserErrors,
                t1 =>
                {
                    Assert.Equal(error.Message, t1.Message);
                    Assert.Equal(error.ErrorType, t1.ErrorType);
                    Assert.Empty(t1.ChildErrors);
                }
            );
            Assert.Collection(result.RawErrors,
                t1 =>
                {
                    Assert.Equal(systemError.Message, t1.Message);
                    Assert.Equal(systemError.ErrorType, t1.ErrorType);
                    Assert.Empty(t1.ChildErrors);
                }
            );
        }

        [Fact]
        public void GivenExceptionWhenLevelOfDetailsIsIncludeUserErrorsThenResponseShouldContainUserErrors()
        {
            // Arrange
            var error = new Error("Error", "Exception");
            var exception = new Exception();
            var optionsMock = Substitute.For<IErrorResponseProviderOptions>();
            var errorProviderMock = Substitute.For<IErrorProvider>();
            var responseCodeProviderMock = Substitute.For<IHttpStatusCodeProvider>();

            optionsMock.ErrorLevelOfDetails.Returns(LevelOfDetails.UserErrors);
            errorProviderMock.ExtractErrorsFor(exception,
                    Arg.Is<IErrorProviderOptions>(o => o.IncludeSystemLevelExceptions == false))
                .Returns(error);

            var errorFormatterMock = Substitute.For<IErrorMessageFormatter>();
            errorFormatterMock.FormatErrorMessages(error)
                .Returns("Formatted Error");

            responseCodeProviderMock.GetHttpStatusCode(Arg.Any<Exception>())
                .Returns(HttpStatusCode.BadRequest);

            var sut = new ErrorResponseProvider(errorProviderMock, errorFormatterMock, responseCodeProviderMock,
                optionsMock);

            // Act
            var result = sut.GetErrors(exception);

            // Assert
            Assert.Equal("Formatted Error", result.Message);
            Assert.Collection(result.UserErrors,
                t1 =>
                {
                    Assert.Equal(error.Message, t1.Message);
                    Assert.Equal(error.ErrorType, t1.ErrorType);
                    Assert.Empty(t1.ChildErrors);
                }
            );
            Assert.Empty(result.RawErrors);
        }

        [Fact]
        public void GivenExceptionWhenLevelOfDetailsIsStandardThenResponseShouldContainMessage()
        {
            // Arrange
            var error = new Error("Error", "Exception");
            var exception = new Exception();
            var optionsMock = Substitute.For<IErrorResponseProviderOptions>();
            var errorProviderMock = Substitute.For<IErrorProvider>();
            var responseCodeProviderMock = Substitute.For<IHttpStatusCodeProvider>();

            optionsMock.ErrorLevelOfDetails.Returns(LevelOfDetails.StandardMessage);
            errorProviderMock.ExtractErrorsFor(exception,
                    Arg.Is<IErrorProviderOptions>(o => o.IncludeSystemLevelExceptions == false))
                .Returns(error);

            var errorFormatterMock = Substitute.For<IErrorMessageFormatter>();
            errorFormatterMock.FormatErrorMessages(error)
                .Returns("Formatted Error");

            responseCodeProviderMock.GetHttpStatusCode(Arg.Any<Exception>())
                .Returns(HttpStatusCode.BadRequest);

            var sut = new ErrorResponseProvider(errorProviderMock, errorFormatterMock, responseCodeProviderMock,
                optionsMock);

            // Act
            var result = sut.GetErrors(exception);

            // Assert
            Assert.Equal("Formatted Error", result.Message);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Empty(result.UserErrors);
            Assert.Empty(result.RawErrors);
        }
    }
}
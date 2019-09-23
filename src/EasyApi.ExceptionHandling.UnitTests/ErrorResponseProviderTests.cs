using System;
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
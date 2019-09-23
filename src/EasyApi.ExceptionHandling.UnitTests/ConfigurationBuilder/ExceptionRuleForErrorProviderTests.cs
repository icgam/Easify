using System;
using System.Collections.Generic;
using EasyApi.ExceptionHandling.ConfigurationBuilder;
using EasyApi.ExceptionHandling.Domain;
using EasyApi.ExceptionHandling.ErrorBuilder;
using EasyApi.ExceptionHandling.UnitTests.Domain;
using NSubstitute;
using Xunit;

namespace EasyApi.ExceptionHandling.UnitTests.ConfigurationBuilder
{
    public sealed class ExceptionRuleForErrorProviderTests
    {
        [Fact]
        public void GivenAppExceptionRuleWhenDerivedExceptionProvidedAndPredicateDoesntMatchThenShouldAdviceToSkip()
        {
            // Arrange
            var sut = new ExceptionRuleForErrorProvider<ApplicationExceptionBase>(f => f.Message.Contains("oo"));
            var exception = new OurApplicationException("Error");

            // Act
            var result = sut.CanHandle(exception);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GivenAppExceptionRuleWhenDerivedExceptionProvidedAndPredicateMatchesThenShouldAdviceToHandle()
        {
            // Arrange
            var sut = new ExceptionRuleForErrorProvider<ApplicationExceptionBase>(f => f.Message.Contains("rr"));
            var exception = new OurApplicationException("Error");

            // Act
            var result = sut.CanHandle(exception);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GivenAppExceptionRuleWhenDerivedExceptionProvidedThenShouldAdviceToHandle()
        {
            // Arrange
            var sut = new ExceptionRuleForErrorProvider<ApplicationExceptionBase>();
            var exception = new OurApplicationException("Error");

            // Act
            var result = sut.CanHandle(exception);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GivenAppExceptionRuleWhenDerivedFromDerivativeExceptionProvidedThenShouldAdviceToHandle()
        {
            // Arrange
            var sut = new ExceptionRuleForErrorProvider<ApplicationExceptionBase>();
            var exception = new OurDerivedApplicationException("Error");

            // Act
            var result = sut.CanHandle(exception);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GivenAppExceptionRuleWhenUnknownExceptionProvidedThenShouldAdviceToSkip()
        {
            // Arrange
            var sut = new ExceptionRuleForErrorProvider<ApplicationExceptionBase>();
            var exception = new ThirdPartyFailureException("Error");

            // Act
            var result = sut.CanHandle(exception);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GivenExceptionWhenCustomBuilderProvidedThenShouldReturnErrorBuiltUsingProvidedBuilder()
        {
            // Arrange
            var builderMock = Substitute.For<IErrorBuilder<ExceptionWithCustomContent>>();
            var sut = new ExceptionRuleForErrorProvider<ExceptionWithCustomContent>(b => b.Use(builderMock));
            var exception = new ExceptionWithCustomContent("Content");
            builderMock.Build(Arg.Any<ExceptionWithCustomContent>(), Arg.Any<IEnumerable<Error>>(), Arg.Any<bool>())
                .Returns(c =>
                {
                    var ex = c.Arg<ExceptionWithCustomContent>();
                    return new Error(ex.Content, ex.GetType().FullName);
                });

            // Act
            var result = sut.GetError(exception, new List<Error>(), false);

            // Assert
            Assert.Equal("Content", result.Message);
            Assert.Equal(typeof(ExceptionWithCustomContent).FullName, result.ErrorType);
            Assert.Empty(result.ChildErrors);
        }

        [Fact]
        public void GivenExceptionWhenRuleCanHandleExceptionTypeThenShouldReturnErrorBuiltUsingDefaultBuilder()
        {
            // Arrange
            var sut = new ExceptionRuleForErrorProvider<Exception>();
            var exception = new OurApplicationException("Error");

            // Act
            var result = sut.GetError(exception, new List<Error>(), false);

            // Assert
            Assert.Equal("Error", result.Message);
            Assert.Equal("OurApplicationException", result.ErrorType);
            Assert.Empty(result.ChildErrors);
        }

        [Fact]
        public void GivenExceptionWhenRuleIsNotAbleToHandleThenShouldThrow()
        {
            // Arrange
            var sut = new ExceptionRuleForErrorProvider<OurApplicationException>();
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => sut.GetError(new Exception(), new List<Error>(), false));
        }
    }
}
using System;
using System.Collections.Generic;
using EasyApi.ExceptionHandling.Domain;
using EasyApi.ExceptionHandling.ErrorBuilder;
using Xunit;

namespace EasyApi.ExceptionHandling.UnitTests.ErrorBuilder
{
    public sealed class DefaultErrorBuilderTests
    {
        [Fact]
        public void GivenAnyExceptionWhenExceptionIsNotSuppliedThenBuilderShouldThrow()
        {
            // Arrange
            var sut = new DefaultErrorBuilder<Exception>();
            
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => sut.Build(null, new List<Error>(), false));
        }

        [Fact]
        public void GivenAnyExceptionWhenInternalErrorsAreNotSuppliedThenBuilderShouldThrow()
        {
            // Arrange
            var sut = new DefaultErrorBuilder<Exception>();
            
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => sut.Build(new Exception(), null, false));
        }

        [Fact]
        public void GivenAnyExceptionWhenErrorIsBuiltThenErrorWithExceptionContentsShouldBeCreated()
        {
            // Arrange
            var sut = new DefaultErrorBuilder<Exception>();
            var exception = new Exception("My Error");
            var internalErrors = new List<Error>();

            // Act
            var result = sut.Build(exception, internalErrors, false);

            // Assert
            Assert.Equal("My Error", result.Message);
            Assert.Equal("Exception", result.ErrorType);
            Assert.Same(internalErrors, result.ChildErrors);
        }
    }
}

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
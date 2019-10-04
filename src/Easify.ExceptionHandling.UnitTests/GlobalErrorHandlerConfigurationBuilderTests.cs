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
using Easify.ExceptionHandling.UnitTests.Domain;
using EasyApi.ExceptionHandling.ConfigurationBuilder;
using EasyApi.ExceptionHandling.Domain;
using EasyApi.ExceptionHandling.Formatter;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace Easify.ExceptionHandling.UnitTests
{
    public sealed class GlobalErrorHandlerConfigurationBuilderTests
    {
        private const string GenericErrorMessage =
            "Unexpected error has occured. Please try again or contact IT support.";

        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        public void GivenBuilderWhenInvalidIndentBySymbolSpecifiedThenThrow(string invalidSymbol)
        {
            // Arrange
            var servicesMock = Substitute.For<IServiceCollection>();
            var sut = new GlobalErrorHandlerConfigurationBuilder(servicesMock);

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => sut.Handle<ApplicationExceptionBase>()
                .UseUserErrors()
                .IndentMessagesUsing(invalidSymbol)
                .Build());
        }

        [Theory]
        [InlineData(" ", "type")]
        [InlineData("", "type")]
        [InlineData(null, "type")]
        [InlineData("message", " ")]
        [InlineData("message", "")]
        [InlineData("message", null)]
        public void GivenBuilderWhenInvalidCustomErrorMessageOrTypeIsSpecifiedThenThrow(string message, string type)
        {
            // Arrange
            var servicesMock = Substitute.For<IServiceCollection>();
            var sut = new GlobalErrorHandlerConfigurationBuilder(servicesMock);

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => sut.Handle<ApplicationExceptionBase>()
                .UseUserErrors()
                .UseGenericError(message, type)
                .Build());
        }

        [Fact]
        public void GivenBuilderWhenApplicationBaseExceptionIsSpecifiedThenGlobalExceptionHandlerShouldBeConfigured()
        {
            // Arrange
            var servicesMock = Substitute.For<IServiceCollection>();
            var sut = new GlobalErrorHandlerConfigurationBuilder(servicesMock);

            // Act
            var result = sut.Handle<ApplicationExceptionBase>().UseStandardMessage().Build();

            // Assert
            Assert.Collection(result.RulesForExceptionHandling,
                t1 => { Assert.Equal(typeof(ApplicationExceptionBase).FullName, t1.TypeFullName); });
        }

        [Fact]
        public void GivenBuilderWhenCustomErrorMessageFormatterSuppliedThenItShouldBeRegistered()
        {
            // Arrange
            var servicesMock = Substitute.For<IServiceCollection>();
            var sut = new GlobalErrorHandlerConfigurationBuilder(servicesMock);

            // Act
            sut.Handle<ApplicationExceptionBase>()
                .UseUserErrors()
                .FormatMessageUsing<CustomErrorMessageFormatter>()
                .Build();

            // Assert
            servicesMock.Received(1)
                .Add(
                    Arg.Is<ServiceDescriptor>(
                        d => d.ServiceType.FullName.Equals(typeof(IErrorMessageFormatter).FullName) &&
                             d.ImplementationType.FullName.Equals(typeof(CustomErrorMessageFormatter).FullName)
                             && d.Lifetime.Equals(ServiceLifetime.Transient)));
        }

        [Fact]
        public void GivenBuilderWhenExceptionTypeIsSpecifiedMultipleTimesThenConfigurationShouldReflectItOnce()
        {
            // Arrange
            var servicesMock = Substitute.For<IServiceCollection>();
            var sut = new GlobalErrorHandlerConfigurationBuilder(servicesMock);

            // Act
            var result = sut.Handle<ApplicationExceptionBase>()
                .AndHandle<ThirdPartyFailureException>()
                .AndHandle<ThirdPartyFailureException>()
                .UseStandardMessage()
                .Build();

            // Assert
            Assert.Equal(2, result.RulesForExceptionHandling.Count);
            Assert.Collection(result.RulesForExceptionHandling,
                t1 => { Assert.Equal(typeof(ApplicationExceptionBase).FullName, t1.TypeFullName); },
                t2 => { Assert.Equal(typeof(ThirdPartyFailureException).FullName, t2.TypeFullName); }
            );
        }

        [Fact]
        public void GivenBuilderWhenExtraExceptionTypesAreSpecifiedThenConfigurationShouldReflectThem()
        {
            // Arrange
            var servicesMock = Substitute.For<IServiceCollection>();
            var sut = new GlobalErrorHandlerConfigurationBuilder(servicesMock);

            // Act
            var result = sut.Handle<ApplicationExceptionBase>()
                .AndHandle<ThirdPartyFailureException>()
                .UseStandardMessage()
                .Build();

            // Assert
            Assert.Collection(result.RulesForExceptionHandling,
                t1 => { Assert.Equal(typeof(ApplicationExceptionBase).FullName, t1.TypeFullName); },
                t2 => { Assert.Equal(typeof(ThirdPartyFailureException).FullName, t2.TypeFullName); }
            );
        }

        [Fact]
        public void GivenBuilderWhenGenericErrorIsSpecifiedThenItShouldBeUsed()
        {
            // Arrange
            var servicesMock = Substitute.For<IServiceCollection>();
            var sut = new GlobalErrorHandlerConfigurationBuilder(servicesMock);

            // Act
            var result = sut.Handle<ApplicationExceptionBase>()
                .UseUserErrors()
                .UseGenericError("Custom Message", "Custom Error Type")
                .Build();

            // Assert
            Assert.Equal("Custom Message", result.GenericError.Message);
            Assert.Equal("Custom Error Type", result.GenericError.ErrorType);
            Assert.Empty(result.GenericError.ChildErrors);
        }

        [Fact]
        public void GivenBuilderWhenIndentBySymbolSpecifiedThenItShouldBeUsed()
        {
            // Arrange
            var servicesMock = Substitute.For<IServiceCollection>();
            var sut = new GlobalErrorHandlerConfigurationBuilder(servicesMock);

            // Act
            var result = sut.Handle<ApplicationExceptionBase>()
                .UseUserErrors()
                .IndentMessagesUsing("*|*")
                .Build();

            // Assert
            Assert.Equal("*|*", result.IndentBy);
        }

        [Fact]
        public void GivenBuilderWhenLevelOfDetailsSetToIncludeAllErrorsThenOptionsShouldHaveSameLevelOfDetails()
        {
            // Arrange
            var servicesMock = Substitute.For<IServiceCollection>();
            var sut = new GlobalErrorHandlerConfigurationBuilder(servicesMock);

            // Act
            var result = sut.Handle<ApplicationExceptionBase>().UseDetailedErrors().Build();

            // Assert
            Assert.Equal(LevelOfDetails.DetailedErrors, result.ErrorLevelOfDetails);
        }

        [Fact]
        public void GivenBuilderWhenLevelOfDetailsSetToIncludeUserErrorsThenOptionsShouldHaveSameLevelOfDetails()
        {
            // Arrange
            var servicesMock = Substitute.For<IServiceCollection>();
            var sut = new GlobalErrorHandlerConfigurationBuilder(servicesMock);

            // Act
            var result = sut.Handle<ApplicationExceptionBase>().UseUserErrors().Build();

            // Assert
            Assert.Equal(LevelOfDetails.UserErrors, result.ErrorLevelOfDetails);
        }

        [Fact]
        public void GivenBuilderWhenLevelOfDetailsSetToStandardThenOptionsShouldHaveSameLevelOfDetails()
        {
            // Arrange
            var servicesMock = Substitute.For<IServiceCollection>();
            var sut = new GlobalErrorHandlerConfigurationBuilder(servicesMock);

            // Act
            var result = sut.Handle<ApplicationExceptionBase>().UseStandardMessage().Build();

            // Assert
            Assert.Equal(LevelOfDetails.StandardMessage, result.ErrorLevelOfDetails);
        }

        [Fact]
        public void GivenBuilderWhenNoGenericErrorGivenThenDefaultErrorShouldBeUsed()
        {
            // Arrange
            var servicesMock = Substitute.For<IServiceCollection>();
            var sut = new GlobalErrorHandlerConfigurationBuilder(servicesMock);

            // Act
            var result = sut.Handle<ApplicationExceptionBase>()
                .UseStandardMessage()
                .Build();

            // Assert
            Assert.Equal(GenericErrorMessage, result.GenericError.Message);
            Assert.Equal(typeof(Exception).Name, result.GenericError.ErrorType);
            Assert.Empty(result.GenericError.ChildErrors);
        }

        [Fact]
        public void GivenBuilderWhenNoIndentBySpecifiedThenDefaultIndenterShouldBeUsed()
        {
            // Arrange
            var servicesMock = Substitute.For<IServiceCollection>();
            var sut = new GlobalErrorHandlerConfigurationBuilder(servicesMock);

            // Act
            var result = sut.Handle<ApplicationExceptionBase>()
                .UseStandardMessage()
                .Build();

            // Assert
            Assert.Equal(" ", result.IndentBy);
        }
    }
}
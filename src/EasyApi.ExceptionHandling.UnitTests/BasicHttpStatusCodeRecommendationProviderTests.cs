using System;
using System.Collections.Generic;
using System.Net;
using EasyApi.ExceptionHandling.ConfigurationBuilder;
using EasyApi.ExceptionHandling.Domain;
using EasyApi.ExceptionHandling.Providers;
using EasyApi.ExceptionHandling.UnitTests.Domain;
using NSubstitute;
using Xunit;

namespace EasyApi.ExceptionHandling.UnitTests
{
    public sealed class BasicHttpStatusCodeRecommendationProviderTests
    {
        [Fact]
        public void GivenExceptionWhenItsDerivativeOfAppExceptionThenHttpCodeShouldBe_400()
        {
            // Arrange
            var optionsMock = Substitute.For<IErrorResponseProviderOptions>();
            var sut = new DefaultHttpStatusCodeProvider(optionsMock);

            optionsMock.RulesForExceptionHandling.Returns(new List<IExceptionRule>
            {
                new ExceptionRuleForErrorProvider<ApplicationExceptionBase>(),
                new ExceptionRuleForErrorProvider<ThirdPartyFailureException>()
            });

            // Act
            var result = sut.GetHttpStatusCode(new OurApplicationException());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result);
        }
        
        [Fact]
        public void GivenExceptionWhenItsDerivativeOfThirdPartyExceptionThenHttpCodeShouldBe_400()
        {
            // Arrange
            var optionsMock = Substitute.For<IErrorResponseProviderOptions>();
            var sut = new DefaultHttpStatusCodeProvider(optionsMock);

            optionsMock.RulesForExceptionHandling.Returns(new List<IExceptionRule>
            {
                new ExceptionRuleForErrorProvider<ApplicationExceptionBase>(),
                new ExceptionRuleForErrorProvider<ThirdPartyFailureException>()
            });

            // Act
            var result = sut.GetHttpStatusCode(new ThirdPartyFailureException());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result);
        }

        [Fact]
        public void GivenExceptionWhenItsSystemExceptionThenHttpCodeShouldBe_500()
        {
            // Arrange
            var optionsMock = Substitute.For<IErrorResponseProviderOptions>();
            var sut = new DefaultHttpStatusCodeProvider(optionsMock);

            optionsMock.RulesForExceptionHandling.Returns(new List<IExceptionRule>
            {
                new ExceptionRuleForErrorProvider<ApplicationExceptionBase>(),
                new ExceptionRuleForErrorProvider<ThirdPartyFailureException>()
            });

            // Act
            var result = sut.GetHttpStatusCode(new Exception());

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result);
        }
    }
}

using EasyApi.AspNetCore.RequestCorrelation.Core.OptionsBuilder;
using Xunit;

namespace EasyApi.AspNetCore.UnitTests.RequestCorrelation.Core.OptionsBuilder
{
    public sealed class UrlRuleTests
    {
        [Theory]
        [InlineData("http://web.com/api/customers", "/api/", true)]
        [InlineData("", "http://web", false)]
        [InlineData("http://web.com/api/customers", "/a", true)]
        [InlineData("http://web.com/api/customers", "api", false)]
        public void GivenUrlRuleWhenStartsWithRuleConfiguredThenShouldCorrectlyMatchValidUrls(string urlToMatch,
            string urlFragment, bool expectedOutcome)
        {
            // Arrange
            var sut = new UrlRule();
            sut.WhenStartsWith(urlFragment);

            // Act
            var result = sut.UrlMatches(urlToMatch);

            // Assert
            Assert.Equal(expectedOutcome, result);
        }

        [Theory]
        [InlineData("http://web.com/api/customers", "pi/customers", true)]
        [InlineData("", "api/customers", false)]
        [InlineData("http://web.com/api/customers", "s", true)]
        [InlineData("http://web.com/api/customers", "pi/customer", false)]
        public void GivenUrlRuleWhenEndsWithRuleConfiguredThenShouldCorrectlyMatchValidUrls(string urlToMatch,
            string urlFragment, bool expectedOutcome)
        {
            // Arrange
            var sut = new UrlRule();
            sut.WhenEndsWith(urlFragment);

            // Act
            var result = sut.UrlMatches(urlToMatch);

            // Assert
            Assert.Equal(expectedOutcome, result);
        }

        [Theory]
        [InlineData("http://web.com/api/customers", "/api/customers", true)]
        [InlineData("http://web.com/api/customers", "/api/customer", true)]
        [InlineData("", "ttp://web.com/api/customers", false)]
        [InlineData("http://web.com/api/customers", "/api/cus", true)]
        public void GivenUrlRuleWhenContainsRuleConfiguredThenShouldCorrectlyMatchValidUrls(string urlToMatch,
            string urlFragment, bool expectedOutcome)
        {
            // Arrange
            var sut = new UrlRule();
            sut.WhenContains(urlFragment);

            // Act
            var result = sut.UrlMatches(urlToMatch);

            // Assert
            Assert.Equal(expectedOutcome, result);
        }
    }
}
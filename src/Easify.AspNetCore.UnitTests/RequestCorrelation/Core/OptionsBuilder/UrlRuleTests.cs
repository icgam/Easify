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

using Easify.AspNetCore.RequestCorrelation.Core.OptionsBuilder;
using Xunit;

namespace Easify.AspNetCore.UnitTests.RequestCorrelation.Core.OptionsBuilder
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
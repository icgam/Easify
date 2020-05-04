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

using Easify.Http;
using Easify.RestEase.Client;
using Easify.Testing;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Easify.RestEase.UnitTests
{
    public class RestClientBuilderTests : IClassFixture<FixtureBase>
    {
        private readonly FixtureBase _fixture;

        private const string AuthorizationHeaderValue = "Bearer AuthorizationHeader";
        private const string CorrelationIdValue = "CorrelationId";

        public RestClientBuilderTests(FixtureBase fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public void GivenProperRequestContext_WhenBuildRestClient_ThenItPopulatesCorrelationIdAndAuthorizationHeader()
        {
            // ARRANGE
            var context = _fixture.Fake<IRequestContext>();
            context.AuthorizationHeader.Returns(AuthorizationHeaderValue);
            context.CorrelationId.Returns(CorrelationIdValue);
            
            var sut = new RestClientBuilder(context);

            // ACT
            var actual = sut.Build<ISampleClient>("http://sampleapi.com");

            // ASSERT
            actual.Authorization.Parameter.Should().Be("AuthorizationHeader");
            actual.Authorization.Scheme.Should().Be("Bearer");
            actual.CorrelationId.Should().Be(CorrelationIdValue);
        }        
        
        [Fact]
        public void GivenRequestContextWithMissingAuthorization_WhenBuildRestClient_ThenItPopulatesCorrelationIdOnly()
        {
            // ARRANGE
            var context = _fixture.Fake<IRequestContext>();
            context.CorrelationId.Returns(CorrelationIdValue);
            
            var sut = new RestClientBuilder(context);

            // ACT
            var actual = sut.Build<ISampleClient>("http://sampleapi.com");

            // ASSERT
            actual.Authorization.Should().BeNull();
            actual.CorrelationId.Should().Be(CorrelationIdValue);
        }     
        
        [Fact]
        public void GivenRequestContextAndExcludeAuthorizationHeader_WhenBuildRestClient_ThenItPopulatesCorrelationIdOnly()
        {
            // ARRANGE
            var context = _fixture.Fake<IRequestContext>();
            context.AuthorizationHeader.Returns(AuthorizationHeaderValue);
            context.CorrelationId.Returns(CorrelationIdValue);
            
            var sut = new RestClientBuilder(context);

            // ACT
            var actual = sut.Build<ISampleClient>("http://sampleapi.com", o => o.ExcludeAuthorizationHeader());

            // ASSERT
            actual.Authorization.Should().BeNull();
            actual.CorrelationId.Should().Be(CorrelationIdValue);
        }
    }
    
    public interface ISampleClient : IRestClient {}
}
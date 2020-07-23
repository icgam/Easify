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
using System.Threading.Tasks;
using Easify.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Polly.Registry;
using Xunit;

namespace Easify.Resilience.UnitTests
{
    public class ResilienceExtensionsTests : IClassFixture<FixtureBase>
    {
        private const string AsyncResiliencePolicy = "AsyncResiliencePolicy";
        private const string ResiliencePolicy = "ResiliencePolicy";
        
        private readonly FixtureBase _fixture;
        private readonly IReadOnlyPolicyRegistry<string> _registry;
        

        public ResilienceExtensionsTests(FixtureBase fixture)
        {
            _fixture = fixture;
            _registry = CreateRegistry();
        }
        
        [Fact]
        public void GivenDefaultRetryPolicy_WhenCallingVoidMethodInRetryMode_ShouldCallServiceThreeTimes()
        {
            // ARRANGE
            var service = _fixture.Fake<IRemoteService>();
            var counter = 0;
            service.When(m => m.Method()).Do(c =>
            {
                counter++;
                if (counter < 3)
                    throw new Exception("Error");
            });
            
            // ACT
            service.ExecuteWithPolicy(s => s.Method(), 
                PolicyFor.RetryPolicy<Exception>(new PolicyRetryOptions()));

            // ASSERT
            service.Received(3).Method();
        }   
        
        [Fact]
        public async Task GivenDefaultRetryPolicy_WhenCallingAsyncMethodInRetryMode_ShouldCallServiceThreeTimes()
        {
            // ARRANGE
            var service = _fixture.Fake<IRemoteService>();
            var counter = 0;
            service.MethodAsync().Returns(c =>
            {
                counter++;
                if (counter < 3)
                    throw new Exception("Error");
                
                return Task.CompletedTask;
            });
            
            // ACT
            await service.ExecuteWithPolicyAsync(s => s.MethodAsync(), 
                PolicyFor.RetryPolicyAsync<Exception>(new PolicyRetryOptions()));

            // ASSERT
            await service.Received(3).MethodAsync();
        }    
        
        [Fact]
        public void GivenDefaultRetryPolicy_WhenCallingMethodWithReturnInRetryMode_ShouldCallServiceThreeTimes()
        {
            // ARRANGE
            var service = _fixture.Fake<IRemoteService>();
            var counter = 0;
            service.MethodWithReturn().Returns(c =>
            {
                counter++;
                if (counter < 3)
                    throw new Exception("Error");

                return 5;
            });
            
            // ACT
            var actual = service.ExecuteWithPolicy(s => s.MethodWithReturn(), 
                PolicyFor.RetryPolicy<Exception>(new PolicyRetryOptions()));

            // ASSERT
            service.Received(3).MethodWithReturn();
            actual.Should().Be(5);
        }   
        
        [Fact]
        public async Task GivenDefaultRetryPolicy_WhenCallingAsyncMethodWithReturnInRetryMode_ShouldCallServiceThreeTimes()
        {
            // ARRANGE
            var service = _fixture.Fake<IRemoteService>();
            var counter = 0;
            service.MethodWithParamAsync().Returns(c =>
            {
                counter++;
                if (counter < 3)
                    throw new Exception("Error");

                return Task.FromResult(5);
            });
            
            // ACT
            var actual = await service.ExecuteWithPolicyAsync(s => s.MethodWithParamAsync(), 
                PolicyFor.RetryPolicyAsync<Exception>(new PolicyRetryOptions()));
        
            // ASSERT
            await service.Received(3).MethodWithParamAsync();
            actual.Should().Be(5);
        }
        
        [Fact]
        public void GivenDefaultRetryPolicy_WhenCallingVoidMethodUsingResilienceStrategy_ShouldCallServiceThreeTimes()
        {
            // ARRANGE
            var service = _fixture.Fake<IRemoteService>();
            var counter = 0;
            service.When(m => m.Method()).Do(c =>
            {
                counter++;
                if (counter < 3)
                    throw new Exception("Error");
            });
            
            // ACT
            service.ExecuteWithPolicy(s => s.Method(), 
                ResiliencePolicy, _registry);

            // ASSERT
            service.Received(3).Method();
        }   
        
        [Fact]
        public async Task GivenDefaultRetryPolicy_WhenCallingAsyncMethodUsingResilienceStrategy_ShouldCallServiceThreeTimes()
        {
            // ARRANGE
            var service = _fixture.Fake<IRemoteService>();
            var counter = 0;
            service.MethodAsync().Returns(c =>
            {
                counter++;
                if (counter < 3)
                    throw new Exception("Error");
                
                return Task.CompletedTask;
            });
            
            // ACT
            await service.ExecuteWithPolicyAsync(s => s.MethodAsync(), 
                AsyncResiliencePolicy, _registry);

            // ASSERT
            await service.Received(3).MethodAsync();
        }    
        
        [Fact]
        public void GivenDefaultRetryPolicy_WhenCallingMethodWithReturnUsingResilienceStrategy_ShouldCallServiceThreeTimes()
        {
            // ARRANGE
            var service = _fixture.Fake<IRemoteService>();
            var counter = 0;
            service.MethodWithReturn().Returns(c =>
            {
                counter++;
                if (counter < 3)
                    throw new Exception("Error");

                return 5;
            });
            
            // ACT
            var actual = service.ExecuteWithPolicy(s => s.MethodWithReturn(), 
                ResiliencePolicy, _registry);

            // ASSERT
            service.Received(3).MethodWithReturn();
            actual.Should().Be(5);
        }   
        
        [Fact]
        public async Task GivenDefaultRetryPolicy_WhenCallingAsyncMethodWithReturnUsingResilienceStrategy_ShouldCallServiceThreeTimes()
        {
            // ARRANGE
            var service = _fixture.Fake<IRemoteService>();
            var counter = 0;
            service.MethodWithParamAsync().Returns(c =>
            {
                counter++;
                if (counter < 3)
                    throw new Exception("Error");

                return Task.FromResult(5);
            });
            
            // ACT
            var actual = await service.ExecuteWithPolicyAsync(s => s.MethodWithParamAsync(), 
                AsyncResiliencePolicy, _registry);
        
            // ASSERT
            await service.Received(3).MethodWithParamAsync();
            actual.Should().Be(5);
        }

        private static IReadOnlyPolicyRegistry<string> CreateRegistry()
        {
            var services = new ServiceCollection();
            services.AddPolicySupport(m =>
                {
                    m.Add(ResiliencePolicy, PolicyFor.ServiceCallResilienceStrategy<Exception>(o => o.CircuitBreaker.NumberOfExceptionsBefore = 4));
                    m.Add(AsyncResiliencePolicy, PolicyFor.ServiceCallResilienceStrategyAsync<Exception>(o => o.CircuitBreaker.NumberOfExceptionsBefore = 4));
                });

            var sp = services.BuildServiceProvider();

            return sp.GetRequiredService<IReadOnlyPolicyRegistry<string>>();
        }
    }
}
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
using Polly;

namespace Easify.Resilience
{
    public class PolicyFor
    {
        public static Policy DataStoreResilienceStrategy<TException>(Action<PolicyOptions> configure = null) where TException : Exception
        {
            var options = new PolicyOptions();
            configure?.Invoke(options);

            return RetryPolicy<TException>(options.Retry);
        }        
        
        public static Policy ServiceCallResilienceStrategy<TException>(Action<PolicyOptions> configure = null) where TException : Exception
        {
            var options = new PolicyOptions();
            configure?.Invoke(options);

            return Policy.Wrap(RetryPolicy<TException>(options.Retry),
                CircuitBreakerPolicy<TException>(options.CircuitBreaker));
        }        
        
        public static AsyncPolicy DataStoreResilienceStrategyAsync<TException>(Action<PolicyOptions> configure = null) where TException : Exception
        {
            var options = new PolicyOptions();
            configure?.Invoke(options);

            return RetryPolicyAsync<TException>(options.Retry);
        }        
        
        public static AsyncPolicy ServiceCallResilienceStrategyAsync<TException>(Action<PolicyOptions> configure = null) where TException : Exception
        {
            var options = new PolicyOptions();
            configure?.Invoke(options);

            return Policy.WrapAsync(RetryPolicyAsync<TException>(options.Retry),
                CircuitBreakerPolicyAsync<TException>(options.CircuitBreaker));
        }

        public static Policy RetryPolicy<TException>(PolicyRetryOptions options) where TException : Exception
        {
            return Policy
                .Handle<TException>()
                .WaitAndRetry(options.WaitsOnRetry, options.OnRetry);
        }          
        
        public static AsyncPolicy RetryPolicyAsync<TException>(PolicyRetryOptions options) where TException : Exception
        {
            return Policy
                .Handle<TException>()
                .WaitAndRetryAsync(options.WaitsOnRetry, options.OnRetry);
        }        
        
        public static Policy CircuitBreakerPolicy<TException>(PolicyCircuitBreakOptions options) where TException : Exception
        {
            return Policy
                .Handle<TException>()
                .CircuitBreaker(options.NumberOfExceptionsBefore, options.DurationOfBreak, options.OnBreak, options.OnReset);
        } 
        
        public static AsyncPolicy CircuitBreakerPolicyAsync<TException>(PolicyCircuitBreakOptions options) where TException : Exception
        {
            return Policy
                .Handle<TException>()
                .CircuitBreakerAsync(options.NumberOfExceptionsBefore, options.DurationOfBreak, options.OnBreak, options.OnReset);
        }
    }
}
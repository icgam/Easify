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
using Polly;
using Polly.Registry;

namespace Easify.Resilience
{
    public static class ResilienceExtensions
    {
        public static void ExecuteWithPolicy<T>(this T instance, Action<T> action, Policy policy)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (policy == null) throw new ArgumentNullException(nameof(policy));
            
            policy.Execute(() => action(instance));
        }        
        
        public static TResult ExecuteWithPolicy<T, TResult>(this T instance, Func<T, TResult> func, Policy policy)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (policy == null) throw new ArgumentNullException(nameof(policy));
            
            return policy.Execute(() => func(instance));
        }     
        
        public static Task ExecuteWithPolicyAsync<T>(this T instance, Func<T, Task> func, AsyncPolicy policy)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (policy == null) throw new ArgumentNullException(nameof(policy));
            
            return policy.ExecuteAsync(() => func(instance));
        }        
        
        public static Task<TResult> ExecuteWithPolicyAsync<T, TResult>(this T instance, Func<T, Task<TResult>> func, AsyncPolicy policy)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (policy == null) throw new ArgumentNullException(nameof(policy));
            
            return policy.ExecuteAsync(() => func(instance));
        }        
        
        public static void ExecuteWithPolicy<T>(this T instance, Action<T> action, string policyName, IReadOnlyPolicyRegistry<string> registry)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (policyName == null) throw new ArgumentNullException(nameof(policyName));
            if (registry == null) throw new ArgumentNullException(nameof(registry));
            
            if (!registry.TryGet<Policy>(policyName, out var policy))
                throw new InvalidPolicyException($"{policyName} hasn't been found in registry");

            policy.Execute(() => action(instance));
        }        
        
        public static TResult ExecuteWithPolicy<T, TResult>(this T instance, Func<T, TResult> func, string policyName, IReadOnlyPolicyRegistry<string> registry)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (policyName == null) throw new ArgumentNullException(nameof(policyName));
            if (registry == null) throw new ArgumentNullException(nameof(registry));
            
            if (!registry.TryGet<Policy>(policyName, out var policy))
                throw new InvalidPolicyException($"{policyName} hasn't been found in registry");
            
            return policy.Execute(() => func(instance));
        }     
        
        public static Task ExecuteWithPolicyAsync<T>(this T instance, Func<T, Task> func, string policyName, IReadOnlyPolicyRegistry<string> registry)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (policyName == null) throw new ArgumentNullException(nameof(policyName));
            if (registry == null) throw new ArgumentNullException(nameof(registry));
            
            if (!registry.TryGet<AsyncPolicy>(policyName, out var policy))
                throw new InvalidPolicyException($"{policyName} hasn't been found in registry");
            
            return policy.ExecuteAsync(() => func(instance));
        }        
        
        public static Task<TResult> ExecuteWithPolicyAsync<T, TResult>(this T instance, Func<T, Task<TResult>> func, string policyName, IReadOnlyPolicyRegistry<string> registry)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (policyName == null) throw new ArgumentNullException(nameof(policyName));
            if (registry == null) throw new ArgumentNullException(nameof(registry));
            
            if (!registry.TryGet<AsyncPolicy>(policyName, out var policy))
                throw new InvalidPolicyException($"{policyName} hasn't been found in registry");
            
            return policy.ExecuteAsync(() => func(instance));
        }
    }
}
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

namespace Easify.Resilience
{
    public static class ResilienceExtensions
    {
        public static void ExecuteWithPolicy<T>(this T instance, Action<T> action, Policy policy)
        {
            policy.Execute(() => action(instance));
        }        
        
        public static TResult ExecuteWithPolicy<T, TResult>(this T instance, Func<T, TResult> func, Policy policy)
        {
            return policy.Execute(() => func(instance));
        }     
        
        public static Task ExecuteWithPolicyAsync<T>(this T instance, Func<T, Task> func, AsyncPolicy policy)
        {
            return policy.ExecuteAsync(() => func(instance));
        }        
        
        public static Task<TResult> ExecuteWithPolicy<T, TResult>(this T instance, Func<T, Task<TResult>> func, AsyncPolicy policy)
        {
            return policy.ExecuteAsync(() => func(instance));
        }
    }
}
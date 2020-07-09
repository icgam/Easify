using System;
using System.Threading.Tasks;
using Polly;

namespace Easify.Resilience
{
    public class FakeService
    {
        public void Method()
        {

        }

        public Task MethodAsync()
        {
            return Task.CompletedTask;
        }        
        
        public int MethodWithParam()
        {
            return 10;
        }

        public Task<int> MethodWithParamAsync()
        {
            return Task.FromResult(10);
        }

        public void SomeMethods()
        {
            var usage = new FakeService();
            usage.ExecuteWithPolicy(u => u.Method(), Policy.NoOp());
            usage.ExecuteWithPolicyAsync(u => u.MethodAsync(), Policy.NoOpAsync());
            usage.ExecuteWithPolicy(u => u.MethodWithParam(), Policy.NoOp());
            usage.ExecuteWithPolicyAsync(u => u.MethodWithParamAsync(), Policy.Handle<Exception>().RetryAsync(3));
        } 
    }
}
using System;
using System.Security.Principal;
using EasyApi.Http;

namespace EasyApi.AspNetCore.Bootstrap.UnitTests
{
    public class DummyRequestContext : IRequestContext
    {
        public string CorrelationId { get; } = Guid.NewGuid().ToString();
        public IPrincipal User { get; } = new GenericPrincipal(new GenericIdentity("Test"), new string[]{});
    }
}

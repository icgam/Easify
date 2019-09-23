using System;
using System.Security.Principal;
using EasyApi.AspNetCore.RequestCorrelation;
using EasyApi.Http;
using Microsoft.AspNetCore.Http;

namespace EasyApi.AspNetCore
{
    public sealed class RequestContext : IRequestContext
    {
        public RequestContext(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null) throw new ArgumentNullException(nameof(httpContextAccessor));
            CorrelationId = httpContextAccessor.HttpContext.Request.GetRequestId();
            User = httpContextAccessor.HttpContext.User;
        }

        public string CorrelationId { get; }
        public IPrincipal User { get; }
    }
}
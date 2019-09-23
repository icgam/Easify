using System;
using EasyApi.Http;
using EasyApi.RestEase;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EasyApi.AspNetCore.Bootstrap.Extensions
{
    public static class RequestContextBuilderExtensions
    {
        public static IRequestContext UseWebContext(this RequestContextBuilder<IServiceProvider> contextBuilder)
        {
            var contextAccessor = contextBuilder.Container.GetRequiredService<IHttpContextAccessor>();

            return new RequestContext(contextAccessor);
        }
    }
}
using System;
using EasyApi.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EasyApi.AspNetCore
{
    public static class RequestContextExtensions
    {
        public static IServiceCollection AddHttpRequestContext(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IRequestContext, RequestContext>();
            services.AddTransient<IOperationContext, RequestContext>();

            return services;
        }
    }
}
using System;
using EasyApi.AspNetCore.RequestCorrelation.Core;
using EasyApi.AspNetCore.RequestCorrelation.Core.OptionsBuilder;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EasyApi.AspNetCore.RequestCorrelation
{
    public static class CorrelateRequestMiddlewareExtensions
    {
        public static IServiceCollection AddRequestCorrelation(this IServiceCollection services)
        {
            var builder = new CorrelationOptionsBuilder().EnforceCorrelation();
            var options = builder.Build();
            services.TryAddSingleton(options);
            services.TryAddSingleton<ICorrelationIdProvider, GuidBasedCorrelationIdProvider>();
            return services;
        }

        public static IServiceCollection AddRequestCorrelation(this IServiceCollection services,
            Func<IExcludeRequests, IBuildOptions> optionsProvider)
        {
            if (optionsProvider == null) throw new ArgumentNullException(nameof(optionsProvider));

            var builder = new CorrelationOptionsBuilder();
            var options = optionsProvider(builder).Build();
            services.TryAddSingleton(options);
            services.TryAddSingleton<ICorrelationIdProvider, GuidBasedCorrelationIdProvider>();
            return services;
        }


        public static IApplicationBuilder UseRequestCorrelation(this IApplicationBuilder app)
        {
            app.UseMiddleware<CorrelateRequestMiddleware>();
            return app;
        }
    }
}
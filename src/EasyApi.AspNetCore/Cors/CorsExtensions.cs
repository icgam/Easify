using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EasyApi.AspNetCore.Cors
{
    // TODO: Extend this to address a policy-based setup for the solution.
    public static class AspNetCorsExtensions
    {
        public const string DefaultCorsPolicyName = "AllowOriginsForAPI";

        public static IServiceCollection AddCorsWithDefaultPolicy(this IServiceCollection services,
            Action<CorsPolicyBuilder> configure)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();

                    configure.Invoke(builder);
                });
            });

            return services;
        }

        public static IServiceCollection AddDefaultCorsPolicy(this IServiceCollection services)
        {
            return AddCorsWithDefaultPolicy(services, builder => { });
        }

        public static IApplicationBuilder UseCorsWithDefaultPolicy(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            return app.UseCors(DefaultCorsPolicyName);
        }
    }
}
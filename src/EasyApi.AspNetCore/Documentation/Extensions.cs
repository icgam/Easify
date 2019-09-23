using System;
using EasyApi.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace EasyApi.AspNetCore.Documentation
{
    public static class Extensions
    {
        public static IServiceCollection AddDefaultApiDocumentation(this IServiceCollection services,
            IConfiguration config)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (config == null) throw new ArgumentNullException(nameof(config));

            var title = config[ConfigurationKeys.AppTitleKey];
            var version = config[ConfigurationKeys.AppVersionKey];

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<RequestCorrelationHeaderFilter>();
                options.SwaggerDoc(version, new Info {Title = title, Version = version});
            });
            return services;
        }

        public static IApplicationBuilder UseDefaultApiDocumentation(this IApplicationBuilder app,
            IConfiguration config)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (config == null) throw new ArgumentNullException(nameof(config));

            // TODO: Avoid duplicating the code for these extensions
            var title = config[ConfigurationKeys.AppTitleKey];
            var version = config[ConfigurationKeys.AppVersionKey];

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint($"/swagger/{version}/swagger.json", title); });

            return app;
        }
    }
}
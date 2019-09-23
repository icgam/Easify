using System;
using EasyApi.AspNetCore.Cors;
using EasyApi.AspNetCore.Documentation;
using EasyApi.AspNetCore.Health;
using EasyApi.AspNetCore.Logging.SeriLog;
using EasyApi.AspNetCore.RequestCorrelation;
using EasyApi.ExceptionHandling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EasyApi.AspNetCore.Bootstrap
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseDefaultApiPipeline(this IApplicationBuilder app,
            IConfiguration configuration,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (env == null) throw new ArgumentNullException(nameof(env));
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

            app.UseCorsWithDefaultPolicy();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseGlobalExceptionHandler();
            }

            app.UseRequestCorrelation();
            app.UseCorrelatedLogs();
            app.UseUserIdentityLogging();
            app.UseHealth();
            app.UseMvc();
            app.UseDefaultApiDocumentation(configuration);

            LogResolvedEnvironment(env, loggerFactory);
        }

        private static void LogResolvedEnvironment(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var log = loggerFactory.CreateLogger("Startup");
            log.LogInformation($"Application is started in '{env.EnvironmentName.ToUpper()}' environemnt ...");
        }
    }
}
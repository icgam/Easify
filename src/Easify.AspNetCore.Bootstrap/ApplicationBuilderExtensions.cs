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
using System.IO;
using System.Linq;
using Easify.AspNetCore.Cors;
using Easify.AspNetCore.Dignostics;
using Easify.AspNetCore.Documentation;
using Easify.AspNetCore.ExceptionHandling;
using Easify.AspNetCore.Logging.SeriLog;
using Easify.AspNetCore.RequestCorrelation;
using Easify.AspNetCore.Security;
using Easify.Configurations;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Easify.AspNetCore.Bootstrap
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseDefaultApiPipeline(this IApplicationBuilder app,
            IConfiguration configuration,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (env == null) throw new ArgumentNullException(nameof(env));
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

            var options = new ApiPipelineOptions(configuration, env, loggerFactory);
            app.UseDefaultApiPipeline(options);
        }

        public static void UseDefaultApiPipeline(this IApplicationBuilder app, ApiPipelineOptions options)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (options == null) throw new ArgumentNullException(nameof(options));

            var appInfo = options.Configuration.GetApplicationInfo();
            var authOptions = options.Configuration.GetAuthOptions();

            if (options.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseGlobalExceptionHandler();
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRequestCorrelation();
            app.UseCorrelatedLogs();
            app.UseRouting();
            app.UseCorsWithDefaultPolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            
            options.PostAuthenticationConfigure?.Invoke();

            app.UseUserIdentityLogging();
            app.UseDiagnostics();
            app.UseOpenApiDocumentation(appInfo, u => u.ConfigureAuth(appInfo, authOptions.Authentication));
            app.UseEndpoints(endpoints =>
            {
                options.PreEndPointsConfigure?.Invoke(endpoints);
                
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapControllers();

                if (options.EnableStartPage)
                    options.StartPageConfigure?.Invoke(endpoints, appInfo);
                
                options.PostEndPointsConfigure?.Invoke(endpoints);
            });

            LogResolvedEnvironment(options.Environment, options.LoggerFactory);
        }
        
        public static void UseStartPage(this IEndpointRouteBuilder endpoints, string applicationName)
        {
            if (endpoints == null) throw new ArgumentNullException(nameof(endpoints));
            if (applicationName == null) throw new ArgumentNullException(nameof(applicationName));

            endpoints.MapGet("/", context =>
            {
                var content = LoadStartPageFromEmbeddedResource(applicationName);
                if (string.IsNullOrEmpty(content))
                    content = applicationName;

                context.Response.ContentType = "text/html";
                return context.Response.WriteAsync(content);
            });
        }

        private static string LoadStartPageFromEmbeddedResource(string applicationName)
        {
            var assembly = typeof(AppBootstrapper<>).Assembly;
            var resourceName = assembly.GetManifestResourceNames().First(s => s.EndsWith("home.html", StringComparison.CurrentCultureIgnoreCase));
            if (string.IsNullOrEmpty(resourceName))
                return null;

            try
            {
                var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                    return null;

                using var reader = new StreamReader(stream);
                var content = reader.ReadToEnd();
                content = content.Replace("{{application}}", applicationName);
                return content;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static void LogResolvedEnvironment(IHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var log = loggerFactory.CreateLogger("Startup");
            log.LogInformation($"Application is started in '{env.EnvironmentName.ToUpper()}' environment ...");
        }
    }
}
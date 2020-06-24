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
using Easify.AspNetCore.Cors;
using Easify.AspNetCore.Dignostics;
using Easify.AspNetCore.Documentation;
using Easify.AspNetCore.ExceptionHandling;
using Easify.AspNetCore.Logging.SeriLog;
using Easify.AspNetCore.RequestCorrelation;
using Easify.AspNetCore.Security;
using Easify.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Easify.AspNetCore.Bootstrap
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

            var appInfo = configuration.GetApplicationInfo();
            var authOptions = configuration.GetAuthOptions();

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

            app.UseAuthentication();
            app.UseRequestCorrelation();
            app.UseCorrelatedLogs();
            app.UseUserIdentityLogging();
            app.UseHealth();
            app.UseMvc();
            app.UseOpenApiDocumentation(appInfo, u => u.ConfigureAuth(appInfo, authOptions.Authentication));

            LogResolvedEnvironment(env, loggerFactory);
        }

        private static void LogResolvedEnvironment(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var log = loggerFactory.CreateLogger("Startup");
            log.LogInformation($"Application is started in '{env.EnvironmentName.ToUpper()}' environemnt ...");
        }
    }
}
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
using Easify.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Easify.AspNetCore.Documentation
{
    public static class DocumentationExtensions
    {
        public static IServiceCollection AddDefaultApiDocumentation(this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var application = configuration.GetApplicationInfo();

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<RequestCorrelationHeaderFilter>();
                options.SwaggerDoc(application.Version, new Info {Title = application.Name, Version = application.Version});
            });
            return services;
        }

        public static IApplicationBuilder UseDefaultApiDocumentation(this IApplicationBuilder app,
            IConfiguration configuration)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var application = configuration.GetApplicationInfo();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint($"/swagger/{application.Version}/swagger.json", application.Name); });

            return app;
        }
    }
}
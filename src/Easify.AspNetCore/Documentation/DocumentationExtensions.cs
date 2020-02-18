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
using System.Collections.Generic;
using Easify.AspNetCore.Security;
using Easify.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Easify.AspNetCore.Documentation
{
    public static class DocumentationExtensions
    {
        public static IApplicationBuilder UseOpenApiDocumentation(this IApplicationBuilder app,
            AppInfo appInfo, Action<SwaggerUIOptions> extend = null)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{appInfo.Version}/swagger.json", appInfo.Name);
                extend?.Invoke(c);
            });

            return app;
        }

        public static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services,
            AppInfo appInfo, Action<SwaggerGenOptions> extend = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (appInfo == null) throw new ArgumentNullException(nameof(appInfo));

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<RequestCorrelationHeaderFilter>();
                options.SwaggerDoc(appInfo.Version, new Info {Title = appInfo.Name, Version = appInfo.Version});

                extend?.Invoke(options);
            });

            return services;
        }

        public static void UseOAuth2Schema(this SwaggerGenOptions options, AuthenticationInfo authentication)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (authentication == null) throw new ArgumentNullException(nameof(authentication));

            var scheme = new OAuth2Scheme
            {
                AuthorizationUrl = $"{authentication.Authority}/oauth2/authorize",
                Description = authentication.Description,
                Flow = "implicit",
                Type = "oauth2",
                TokenUrl = $"{authentication.Authority}/oauth2/v2.0/token",
                Scopes = new Dictionary<string, string>
                {
                    {"user_impersonation", "Access API"}
                }
            };

            options.AddSecurityDefinition("oauth2", scheme);
        }

        public static void ConfigureOAuth2(this SwaggerUIOptions options, AppInfo appInfo, AuthenticationInfo authentication)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (authentication == null) throw new ArgumentNullException(nameof(authentication));
            
            options.OAuthClientId(authentication.Audience);
            options.OAuthClientSecret(authentication.AudienceSecret);
            options.OAuthAppName(appInfo.Name);
            options.OAuthRealm(authentication.Audience); // TODO: WTF?
            options.OAuthScopeSeparator(" ");
            options.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { {"resource", authentication.Audience} }); 
            options.OAuthUseBasicAuthenticationWithAccessCodeGrant();
        }
    }
}
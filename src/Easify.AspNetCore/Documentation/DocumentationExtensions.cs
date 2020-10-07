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
using Easify.AspNetCore.Security.Impersonation;
using Easify.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Easify.AspNetCore.Documentation
{
    public static class DocumentationExtensions
    {
        private static readonly Dictionary<AuthenticationMode, Action<SwaggerGenOptions, AuthenticationInfo>> Configurator 
            = new Dictionary<AuthenticationMode, Action<SwaggerGenOptions, AuthenticationInfo>>
            {
                {AuthenticationMode.None, (sg, auth) => {}},
                {AuthenticationMode.Impersonated, (sg, auth) => sg.UseApiKeyScheme(auth)},
                {AuthenticationMode.OAuth2, (sg, auth) => sg.UseOAuth2Scheme(auth)},
            };
        
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
            AppInfo appInfo, AuthOptions authOptions)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (authOptions == null) throw new ArgumentNullException(nameof(authOptions));

            if (!Configurator.ContainsKey(authOptions.AuthenticationMode))
                throw new InvalidAuthenticationModeException(authOptions.AuthenticationMode);

            var action = Configurator[authOptions.AuthenticationMode];
            services.AddOpenApiDocumentation(appInfo, o => action(o, authOptions.Authentication));

            return services;
        }

        public static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services,
            AppInfo appInfo, Action<SwaggerGenOptions> extend = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (appInfo == null) throw new ArgumentNullException(nameof(appInfo));

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<RequestCorrelationHeaderFilter>();
                options.SwaggerDoc(appInfo.Version, new OpenApiInfo() {Title = appInfo.Name, Version = appInfo.Version});

                extend?.Invoke(options);
            });

            return services;
        }        
        
        public static void UseOAuth2Scheme(this SwaggerGenOptions options, AuthenticationInfo authentication)
        {
            if (authentication?.Authority == null)
                throw new ApiDocumentationException("Invalid or missing Authentication info: Authority");
                
            var scheme = new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{authentication.Authority}/oauth2/authorize"),
                        Scopes = new Dictionary<string, string>(),
                    }
                }, 
                Description = authentication.Description
            };
            options.AddSecurityDefinition("oauth2", scheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                    },
                    new string[] { }
                }
            });
        }
        
        public static void UseApiKeyScheme(this SwaggerGenOptions options, AuthenticationInfo authentication)
        {
            var scheme = new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = authentication.Description
            };
            options.AddSecurityDefinition(ImpersonationBearerDefaults.AuthenticationScheme, scheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = ImpersonationBearerDefaults.AuthenticationScheme }
                    },
                    new string[] { }
                }
            });
        }

        public static void ConfigureAuth(this SwaggerUIOptions options, AppInfo appInfo,
            AuthenticationInfo authentication)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (authentication == null) throw new ArgumentNullException(nameof(authentication));

            options.OAuthClientId(authentication.Audience);
            options.OAuthAppName(appInfo.Name);
            options.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
            {
                {"resource", authentication.Audience}
            });
        }
    }
}
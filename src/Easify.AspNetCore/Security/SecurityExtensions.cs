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
using Easify.AspNetCore.Security.Impersonation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace Easify.AspNetCore.Security
{
    public static class SecurityExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, AuthOptions options)
        {
            switch (options.AuthenticationMode)
            {
                case AuthenticationMode.None:
                    break;
                case AuthenticationMode.Impersonated:
                    services.AddAuthentication(ImpersonationBearerDefaults.AuthenticationScheme)
                        .AddImpersonationBearer();
                    break;
                case AuthenticationMode.OAuth2:
                    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(opt =>
                        {
                            opt.Authority = options.Authentication.Authority;
                            opt.Audience = options.Authentication.Audience;
                        });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return services;
        }

        private static AuthenticationBuilder AddImpersonationBearer(this AuthenticationBuilder builder,
            Action<ImpersonationBearerOptions> configOptions = null)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddScheme<ImpersonationBearerOptions, ImpersonationAuthenticationHandler>(ImpersonationBearerDefaults.AuthenticationScheme,
                configOptions);

            return builder;
        }
    }
}
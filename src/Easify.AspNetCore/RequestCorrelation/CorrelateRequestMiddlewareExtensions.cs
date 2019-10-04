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
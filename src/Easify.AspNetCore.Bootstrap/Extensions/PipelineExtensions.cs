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
using AutoMapper;
 using Easify.Bootstrap;
 using Easify.DependencyInjection;
 using EasyApi.Extensions;
using EasyApi.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EasyApi.AspNetCore.Bootstrap.Extensions
{
    public static class PipelineExtensions
    {
        public static IExtendPipeline ReplaceRequestContext<TContext>(this IExtendPipeline bootstrapper)
            where TContext : class, IRequestContext, new()
        {
            return bootstrapper.Extend((services, config) =>
            {
                services.ReplaceFirst<IRequestContext, TContext>(ServiceLifetime.Transient);
            });
        }

        public static IExtendPipeline ReplaceRequestContext<TContext>(this IExtendPipeline bootstrapper,
            Func<IServiceProvider, TContext> requestContextProvider) where TContext : class, IRequestContext
        {
            if (requestContextProvider == null) throw new ArgumentNullException(nameof(requestContextProvider));

            return bootstrapper.Extend((services, config) =>
            {
                services.RemoveFirstOrNothing<IRequestContext>();
                services.AddTransient<IRequestContext>(requestContextProvider);
            });
        }

        public static IExtendPipeline ConfigureMappings(this IExtendPipeline bootstrapper,
            Action<IMapperConfigurationExpression> mappingsConfiguration)
        {
            bootstrapper.Extend((services, config) =>
            {
                services.AddSingleton<ITypeMapper>(c =>
                {
                    var componentResolver = c.GetService<IComponentResolver>();
                    return new DefaultTypeMapper(mappingsConfiguration, componentResolver);
                });
            });

            return bootstrapper;
        }
    }
}
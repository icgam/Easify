using System;
using AutoMapper;
using EasyApi.Bootstrap;
using EasyApi.DependencyInjection;
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
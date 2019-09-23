using System;
using System.Collections.Generic;
using System.Reflection;
using EasyApi.AspNetCore.ActionFilters;
using EasyApi.AspNetCore.Serializations;
using EasyApi.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace EasyApi.AspNetCore.Mvc
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddDefaultJsonOptions(this IMvcBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddJsonOptions(o => o.SerializerSettings.ConfigureJsonSettings());

            return builder;
        }
    }

    public static class MvcServiceCollectionExtensions
    {
        private const string AssemblyNameSectionSeparator = ".";

        public static IServiceCollection AddDefaultMvc<TStartup>(this IServiceCollection services)
            where TStartup : class
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var prefix = typeof(TStartup).GetAssemblyPrefix(AssemblyNameSectionSeparator);
            services.AddMvc(options =>
                {
                    options.Filters.Add(typeof(LoggingActionFilter));
                    options.Filters.Add(typeof(ValidateModelStateActionFilter));
                })
                .AddJsonOptions(o => o.SerializerSettings.ConfigureJsonSettings())
                .AddFluentValidation(fv => fv.RegisterValidatorsFromDomain<TStartup>(prefix));

            return services;
        }

        public static void RegisterValidatorsFromDomain<T>(this FluentValidationMvcConfiguration config,
            string assemblyNameStartsWith) where T : class
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (string.IsNullOrWhiteSpace(assemblyNameStartsWith))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(assemblyNameStartsWith));

            var assemblies = GetReferencedAssembliesFromType<T>(assemblyNameStartsWith);

            foreach (var assembly in assemblies) config.RegisterValidatorsFromAssembly(assembly);
        }

        private static IEnumerable<Assembly> GetReferencedAssembliesFromType<T>(string assemblyNameStartsWith)
        {
            var type = typeof(T);
            return type.GetReferencedAssemblies(assemblyNameStartsWith);
        }
    }
}
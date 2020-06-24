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
using System.Reflection;
using Easify.AspNetCore.ActionFilters;
using Easify.AspNetCore.Serializations;
using Easify.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Easify.AspNetCore.Mvc
{
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
                .AddFluentValidation(fv => fv.RegisterValidatorsFromDomain<TStartup>(prefix))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            return services;
        }

        private static void RegisterValidatorsFromDomain<T>(this FluentValidationMvcConfiguration config,
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
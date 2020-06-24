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
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Easify.AspNetCore
{
    public static class ConfigurationBuilderExtensions
    {
        public const string EnvironmentVariablePrefix = "EASIFY_";
        public static IConfigurationBuilder ConfigureBuilder(this IConfigurationBuilder builder, ConfigurationOptions options)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (options == null) throw new ArgumentNullException(nameof(options));

            builder
                .AddJsonFile(Path.Combine(options.BasePath, "appSettings.json"), false, true)
                .AddJsonFile(Path.Combine(options.BasePath, $"appsettings.{options.Environment}.json"), true, true);
                
            if (options.IsDevelopment)
            {
                var appAssembly = Assembly.Load(new AssemblyName(options.AppEntry));
                if (appAssembly != null)
                {
                    builder.AddUserSecrets(appAssembly, optional: true);
                }
            }   
                
            builder
                .AddEnvironmentVariables(EnvironmentVariablePrefix)
                .AddCommandLine(options.Args);

            return builder;
        }
    }
}
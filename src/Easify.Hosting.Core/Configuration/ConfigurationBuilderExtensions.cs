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
using Microsoft.Extensions.Configuration;

namespace Easify.Hosting.Core.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public const string EnvironmentVariablePrefix = "EASIFY_";
        public static IConfigurationBuilder ConfigureBuilder(this IConfigurationBuilder builder, string basePath, string environment, string[] args)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (basePath == null) throw new ArgumentNullException(nameof(basePath));
            if (environment == null) throw new ArgumentNullException(nameof(environment));
            if (args == null) throw new ArgumentNullException(nameof(args));
            
            builder.SetBasePath(basePath)
                .AddJsonFile(Path.Combine(basePath, "appSettings.json"), false, true)
                .AddJsonFile(Path.Combine(basePath, $"appsettings.{environment}.json"), true, true)
                .AddEnvironmentVariables(EnvironmentVariablePrefix)
                .AddCommandLine(args);

            return builder;
        }
    }
}
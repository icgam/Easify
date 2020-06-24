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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Easify.AspNetCore;
using Microsoft.Extensions.Configuration;

namespace Easify.Hosting.Core.Configuration
{
    public sealed class HostingOptionsProvider
    {
        private const string InteractiveFlag = "--interactive";

        public HostingOptions GetHostingOptions(string[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var isService = LaunchedAsService(args);
            var basePath = GetPathToContentRoot(isService);
            var appEntryName = GetEntryName(isService);
            var configuration = LoadConfiguration(basePath, appEntryName, args);

            return new HostingOptions(basePath, isService, configuration);
        }

        private IConfiguration LoadConfiguration(string basePath, string appEntryName, string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .ConfigureBuilder(new ConfigurationOptions(basePath, environment, appEntryName, args))
                .Build();

            return configuration;
        }

        private static bool LaunchedAsService(IEnumerable<string> args)
        {
            return (Debugger.IsAttached || args.Contains(InteractiveFlag)) == false;
        }

        private static string GetPathToContentRoot(bool isService)
        {
            if (!isService) 
                return Directory.GetCurrentDirectory();
            
            var pathToExe = Process.GetCurrentProcess().MainModule?.FileName;
            return Path.GetDirectoryName(pathToExe);
        }        
        
        private static string GetEntryName(bool isService)
        {
            if (!isService) 
                return Assembly.GetEntryAssembly()?.GetName().Name;
            
            var pathToExe = Process.GetCurrentProcess().MainModule?.FileName;
            return Path.GetFileName(pathToExe);
        }
    }
}
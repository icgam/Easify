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
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace EasyApi.Hosting.Core.Configuration
{
    public sealed class HostingOptionsProvider
    {
        private const string InteractiveFlag = "--interactive";

        public HostingOptions GetHostingOptions(string[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var isService = LaunchedAsService(args);
            var pathToContentRoot = GetPathToContentRoot(isService);
            var configuration = LoadConfiguration(pathToContentRoot, args);

            return new HostingOptions(pathToContentRoot, isService, configuration);
        }

        private IConfiguration LoadConfiguration(string pathToContentRoot, string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(pathToContentRoot, "appSettings.json"), false, true)
                .AddJsonFile(Path.Combine(pathToContentRoot, $"appsettings.{environmentName}.json"), true, true)
                .AddCommandLine(args)
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            return configuration;
        }

        private bool LaunchedAsService(string[] args)
        {
            return (Debugger.IsAttached || args.Contains(InteractiveFlag)) == false;
        }

        private string GetPathToContentRoot(bool isService)
        {
            var pathToContentRoot = Directory.GetCurrentDirectory();
            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                pathToContentRoot = Path.GetDirectoryName(pathToExe);
            }
            return pathToContentRoot;
        }
    }
}
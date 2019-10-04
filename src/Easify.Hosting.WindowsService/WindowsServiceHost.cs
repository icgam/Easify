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
using EasyApi.AspNetCore.Logging.SeriLog.Fluent;
using EasyApi.Hosting.Core;
using EasyApi.Hosting.Core.Configuration;
using EasyApi.Hosting.Core.HostContainer;
using Microsoft.AspNetCore.Hosting;

namespace EasyApi.Hosting.WindowsService
{
    public static class WindowsServiceHost
    {
        private static void Build<TStartup>(Func<IWebHost, IServiceHost> serviceContainerBuilder,
            Func<ILoggerBuilder, IBuildLogger> loggerBuilderFactory, string[] args) where TStartup : class
        {
            var options = new HostingOptionsProvider().GetHostingOptions(args);
            var hostBuilder = new ServiceHostBuilder<TStartup>(options, serviceContainerBuilder);
            var host = hostBuilder.Build(loggerBuilderFactory, args);
            host.Run();
        }

        public static void Run<TStartup>(Func<IWebHost, IServiceHost> serviceContainerBuilder,
            Func<ILoggerBuilder, IBuildLogger> loggerBuilderFactory) where TStartup : class
        {
            Run<TStartup>(serviceContainerBuilder, loggerBuilderFactory, new string[] { });
        }

        public static void Run<TStartup>(Func<IWebHost, IServiceHost> serviceContainerBuilder,
            Func<ILoggerBuilder, IBuildLogger> loggerBuilderFactory, string[] args) where TStartup : class
        {
            Build<TStartup>(serviceContainerBuilder, loggerBuilderFactory, args);
        }
    }
}
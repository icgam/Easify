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
using Easify.AspNetCore.Logging.SeriLog.Fluent;
using Easify.Hosting.Core;
using Easify.Hosting.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Easify.Hosting.WindowsService
{
    public static class HostAsService
    {
        private static void Build<TStartup>(Func<IWebHost, IHostContainer> serviceContainerBuilder,
            Func<ILoggerBuilder, IBuildLogger> loggerBuilderFactory, string[] args) where TStartup : class
        {
            var options = new HostingOptionsProvider().GetHostingOptions(args);

            var webHost = new WebHostBuilder()
                .UseContentRoot(options.PathToContentRoot)
                .UseUrls(options.BaseUrl)
                .UseConfiguration(options.Configuration)
                .UseStartup<TStartup>()
                .UseSerilog((context, configuration) =>
                {
                    loggerBuilderFactory(new LoggerBuilder(context, configuration)).Build<TStartup>();
                })
                .Build();

            var serviceHost = options.LaunchedAsService
                ? new HostAsServiceContainer(webHost)
                : (IHostContainer)new HostAsWebContainer(webHost);

            serviceHost.Run();
        }

        public static void Run<TStartup>(Func<IWebHost, IHostContainer> serviceContainerBuilder,
            Func<ILoggerBuilder, IBuildLogger> loggerBuilderFactory) where TStartup : class
        {
            Run<TStartup>(serviceContainerBuilder, loggerBuilderFactory, new string[] { });
        }

        public static void Run<TStartup>(Func<IWebHost, IHostContainer> serviceContainerBuilder,
            Func<ILoggerBuilder, IBuildLogger> loggerBuilderFactory, string[] args) where TStartup : class
        {
            Build<TStartup>(serviceContainerBuilder, loggerBuilderFactory, args);
        }
    }
}

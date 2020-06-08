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
using Easify.Hosting.Core.Configuration;
using Easify.Hosting.Core.HostContainer;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Easify.Hosting.Core
{
    //TODO: This should be revised regarding to the new capabilities in v2.0
    public sealed class ServiceHostBuilder<TStartup> where TStartup : class
    {
        private readonly HostingOptions _hostingOptions;
        private readonly Func<IWebHost, IServiceHost> _serviceContainerBuilder;

        public ServiceHostBuilder(HostingOptions hostingOptions, Func<IWebHost, IServiceHost> serviceContainerBuilder)
        {
            _serviceContainerBuilder = serviceContainerBuilder ??
                                       throw new ArgumentNullException(nameof(serviceContainerBuilder));
            _hostingOptions = hostingOptions ?? throw new ArgumentNullException(nameof(hostingOptions));
        }

        public IServiceHost Build(Func<ILoggerBuilder, IBuildLogger> loggerBuilderFactory, string[] args)
        {
            var webHost = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(_hostingOptions.PathToContentRoot)
                .UseUrls(_hostingOptions.BaseUrl)
                .UseConfiguration(_hostingOptions.Configuration)
                .UseStartup<TStartup>()
                .UseSerilog((context, configuration) =>
                {
                    var loggerBuilder = loggerBuilderFactory(new LoggerBuilder(context, configuration));
                    loggerBuilder.Build<TStartup>();
                })
                .Build();

            return _hostingOptions.LaunchedAsService
                ? _serviceContainerBuilder(webHost)
                : new WebHostContainer(webHost);
        }
    }
}
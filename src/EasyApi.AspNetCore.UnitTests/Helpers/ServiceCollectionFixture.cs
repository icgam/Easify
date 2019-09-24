// This software is part of the EasyApi framework
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

﻿using System;
using EasyApi.AspNetCore.Logging.SeriLog;
using EasyApi.Logging;
using Foil;
using Foil.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;

namespace EasyApi.AspNetCore.UnitTests.Helpers
{
    public class ServiceCollectionFixture
    {
        public InMemoryLogger<LogInterceptor> Log { get; private set; }

        public IServiceProvider BuildProvider(Func<IServiceCollection, IServiceCollection> serviceConfigurator,
            Func<LogLevel, bool> logLevelFilter)
        {
            if (serviceConfigurator == null) throw new ArgumentNullException(nameof(serviceConfigurator));
            if (logLevelFilter == null) throw new ArgumentNullException(nameof(logLevelFilter));

            var services = new ServiceCollection();

            Log = new InMemoryLogger<LogInterceptor>(typeof(ILogger).Name, (s, level) => logLevelFilter(level));
            services.AddTransient<IArgumentsFormatter, ArgumentsFormatter>();
            services.AddSingleton(p => new ArgumentFormatterOptions
            {
                Formatting = Formatting.Indented
            });
            services.AddSingleton(p => Log);
            services.AddSingleton<ILogger<LogInterceptor>>(p => Log);
            services.AddSingleton(p => Substitute.For<ILogger<ArgumentsFormatter>>());
            serviceConfigurator(services);

            return services.BuildServiceProvider();
        }

        public IServiceProvider BuildProvider(Func<LogLevel, bool> logLevelFilter)
        {
            return BuildProvider(s =>
                    s.AddTransientWithInterception<IFakeService, FakeService>(m =>
                        m.InterceptBy<LogInterceptor>().UseMethodConvention<NonQueryMethodsConvention>()),
                logLevelFilter);
        }
    }
}
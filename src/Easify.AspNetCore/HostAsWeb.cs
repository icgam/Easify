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
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Easify.AspNetCore
{
    public static class HostAsWeb
    {
        private static IWebHost Build<TStartup>(Func<ILoggerBuilder, IBuildLogger> loggerBuilderFactory, string[] args)
            where TStartup : class
        {
            if (loggerBuilderFactory == null) throw new ArgumentNullException(nameof(loggerBuilderFactory));

            var host = WebHost
                .CreateDefaultBuilder<TStartup>(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    var options = new ConfigurationOptions(env.ContentRootPath, env.EnvironmentName,
                        env.ApplicationName, args);
                    config.ConfigureBuilder(options);
                })
                .UseSerilog((context, configuration) =>
                {
                    loggerBuilderFactory(new LoggerBuilder(context, configuration)).Build<TStartup>();
                })
                .Build();

            return host;
        }

        public static void Run<TStartup>(Func<ILoggerBuilder, IBuildLogger> loggerBuilderFactory) where TStartup : class
        {
            Run<TStartup>(loggerBuilderFactory, new string[] { });
        }

        public static void Run<TStartup>(Func<ILoggerBuilder, IBuildLogger> loggerBuilderFactory, string[] args)
            where TStartup : class
        {
            Build<TStartup>(loggerBuilderFactory, args).Run();
        }
    }
}

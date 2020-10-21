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
using System.Net.Http;
using Easify.AspNetCore;
using Easify.AspNetCore.Logging.SeriLog.Fluent;
using Easify.Configurations;
using Easify.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Easify.Sample.WebAPI.IntegrationTests.Helpers
{
    public sealed class TestApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private const string LogsDirectoryPattern = "Logs\\{0}";
        private const int LoggerFlushDelayInMs = 1000;

        private readonly string _logFilePattern =
            $"Easify.Sample.WebAPI.IntegrationTests-{EnvironmentNames.Integration}" + "-{0}.log";

        private readonly TestApplicationOptions _options;

        public TestApplicationFactory(TestApplicationOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        private Guid SessionId { get; } = Guid.NewGuid();

        public string LogFilePath
        {
            get
            {
                var logsDirectory = LogDirectoryPath;
                var fileName = string.Format(_logFilePattern, DateTime.Today.ToString("yyyyMMdd"));
                return Path.Combine(logsDirectory, fileName);
            }
        }

        public string LogDirectoryPath
        {
            get
            {
                var path = Directory.GetCurrentDirectory();
                var logsDirectory = string.Format(LogsDirectoryPattern, SessionId);
                return Path.Combine(path, logsDirectory);
            }
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            var hostBuilder = WebHost.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = context.HostingEnvironment;
                    env.EnvironmentName = _options.Environment;

                    var configOptions = new ConfigurationOptions(env.ContentRootPath, env.EnvironmentName,
                        env.ApplicationName, new string[] { });
                    builder.ConfigureBuilder(configOptions);
                })
                .UseStartup<TStartup>()
                .ConfigureServices(_options.ConfigureServices);

            if (_options.EnableLoggingToFile)
                hostBuilder.UseSerilog((context, configuration) =>
                {
                    var loggerBuilder =
                        new LoggerBuilder(context.HostingEnvironment, context.Configuration, configuration)
                            .ConfigureLogger<TStartup>(c =>
                                c.FlushToDiskEveryInMs(1).SaveLogsTo(LogFilePath));
                    loggerBuilder.Build<TStartup>();
                });

            return hostBuilder;
        }

        protected override void ConfigureClient(HttpClient client)
        {
            base.ConfigureClient(client);
            client.AddRequestIdToHeader(Guid.NewGuid().ToString());
        }

        public static TestApplicationFactory<TStartup> CreateWithLoggingEnabled()
        {
            return new TestApplicationFactory<TStartup>(new TestApplicationOptions
            {
                EnableLoggingToFile = true
            });
        }

        public static TestApplicationFactory<TStartup> Create(string environment = EnvironmentNames.Integration)
        {
            return new TestApplicationFactory<TStartup>(new TestApplicationOptions
            {
                Environment = environment
            });
        }

        public static TestApplicationFactory<TStartup> Create(Action<IServiceCollection> configureServices,
            string environment = EnvironmentNames.Integration)
        {
            return new TestApplicationFactory<TStartup>(new TestApplicationOptions
            {
                Environment = environment,
                ConfigureServices = configureServices
            });
        }
    }
}
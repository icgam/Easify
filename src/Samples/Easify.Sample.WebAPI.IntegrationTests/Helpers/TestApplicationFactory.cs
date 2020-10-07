using System;
using System.IO;
using System.Net.Http;
using Easify.AspNetCore;
using Easify.AspNetCore.Logging.SeriLog.Fluent;
using Easify.Configurations;
using Easify.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
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

namespace Easify.Sample.WebAPI.IntegrationTests.Helpers
{
    public sealed class TestApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup: class
    {
        private readonly TestApplicationOptions _options;
        private const string LogsDirectoryPattern = "Logs\\{0}";
        private const int LoggerFlushDelayInMs = 1000;

        private readonly string _logFilePattern =
            $"Easify.Sample.WebAPI.IntegrationTests-{EnvironmentNames.Integration}" + "-{0}.log";

        public TestApplicationFactory(TestApplicationOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            var sessionId = Guid.NewGuid();
            var hostBuilder = base.CreateWebHostBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = context.HostingEnvironment;
                    env.EnvironmentName = _options.Environment;

                    var configOptions = new ConfigurationOptions(env.ContentRootPath, env.EnvironmentName,
                        env.ApplicationName, new string[] { });
                    builder.ConfigureBuilder(configOptions);
                })
                .ConfigureServices(_options.ConfigureServices);
            
            if (_options.EnableLoggingToFile)
                hostBuilder.UseSerilog((context, configuration) =>
                {
                    var loggerBuilder = new LoggerBuilder(context, configuration).ConfigureLogger<TStartup>(c =>
                        c.FlushToDiskEveryInMs(1).SaveLogsTo(LogFilePath(sessionId)));
                    loggerBuilder.Build<TStartup>();
                });

            return hostBuilder;
        }

        protected override void ConfigureClient(HttpClient client)
        {
            base.ConfigureClient(client);
            client.AddRequestIdToHeader(Guid.NewGuid().ToString());
        }
        
        public static TestServerFixture<TStartup> CreateWithLoggingEnabled()
        {
            return new TestServerFixture<TStartup>(new TestApplicationOptions
            {
                EnableLoggingToFile = true
            });
        }

        public static TestServerFixture<TStartup> Create(string environment = EnvironmentNames.Integration)
        {
            return new TestServerFixture<TStartup>(new TestApplicationOptions
            {
                Environment = environment
            });
        }

        public static TestServerFixture<TStartup> Create(Action<IServiceCollection> configureServices,
            string environment = EnvironmentNames.Integration)
        {
            return new TestServerFixture<TStartup>(new TestApplicationOptions
            {
                Environment = environment,
                ConfigureServices = configureServices
            });
        }

        private string LogFilePath(Guid sessionId)
        {
            var logsDirectory = LogDirectoryPath(sessionId);
            var fileName = string.Format(_logFilePattern, DateTime.Today.ToString("yyyyMMdd"));
            return Path.Combine(logsDirectory, fileName);
        }

        private static string LogDirectoryPath(Guid sessionId)
        {
            var path = Directory.GetCurrentDirectory();
            var logsDirectory = string.Format(LogsDirectoryPattern, sessionId);
            return Path.Combine(path, logsDirectory);
        }
    }
}
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
using System.Threading;
using Easify.AspNetCore;
using Easify.AspNetCore.Logging.SeriLog.Fluent;
using Easify.Configurations;
using Easify.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Easify.Sample.WebAPI.IntegrationTests.Helpers
{
    public sealed class TestServerFixture<TStartup> : IDisposable where TStartup : class
    {
        private const string LogsDirectoryPattern = "Logs\\{0}";
        private const int LoggerFlushDelayInMs = 1000;

        private readonly string LogFilePattern =
            $"Easify.Sample.WebAPI.IntegrationTests-{EnvironmentNames.Integration}" + "-{0}.log";

        public TestServerFixture(TestApplicationOptions options)
        {
            SessionId = Guid.NewGuid();

            var hostBuilder = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = context.HostingEnvironment;
                    env.EnvironmentName = options.Environment;
                    
                    var configOptions = new ConfigurationOptions(env.ContentRootPath, env.EnvironmentName, env.ApplicationName, new string[] {});
                    builder.ConfigureBuilder(configOptions);
                })
                .UseStartup<TStartup>()
                .ConfigureServices(options.ConfigureServices);

            if (options.EnableLoggingToFile)
                hostBuilder.UseSerilog((context, configuration) =>
                {
                    var loggerBuilder =
                        new LoggerBuilder(context, configuration).ConfigureLogger<TStartup>(c =>
                            c.FlushToDiskEveryInMs(1).SaveLogsTo(LogDirectoryPath));
                    loggerBuilder.Build<TStartup>();
                });

            Server = new TestServer(hostBuilder);

            Client = Server.CreateClient();
            Client.AddRequestIdToHeader(Guid.NewGuid().ToString());
        }

        public HttpClient Client { get; }

        public TestServer Server { get; }

        public string LogFilePath
        {
            get
            {
                var logsDirectory = LogDirectoryPath;
                var fileName = string.Format(LogFilePattern, DateTime.Today.ToString("yyyyMMdd"));
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

        private Guid SessionId { get; }

        public void Dispose()
        {
            try
            {
                Client?.Dispose();
                Server?.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            {
                Thread.Sleep(LoggerFlushDelayInMs);
                Log.CloseAndFlush();
                Thread.Sleep(LoggerFlushDelayInMs);
            }
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
    }
}
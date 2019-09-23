using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using EasyApi.AspNetCore.Logging.SeriLog.Fluent;
using EasyApi.Configurations;
using EasyApi.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace EasyApi.Sample.WebAPI.IntegrationTests.Helpers
{
    public sealed class TestServerFixture<TStartup> : IDisposable where TStartup : class
    {
        private const string LogsDirectoryPattern = "Logs\\{0}";
        private const int LoggerFlushDelayInMs = 1000;

        private readonly string LogFilePattern =
            $"EasyApi.Sample.WebAPI.IntegrationTests-{EnvironmentNames.Integration}" + "-{0}.log";

        public TestServerFixture(TestServerOptions options)
        {
            SessionId = Guid.NewGuid();

            var hostBuilder = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = context.HostingEnvironment;
                    env.EnvironmentName = options.Environment;

                    builder.SetBasePath(env.ContentRootPath)
                        .AddJsonFile("appsettings.json", false, true);
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
            return new TestServerFixture<TStartup>(new TestServerOptions
            {
                EnableLoggingToFile = true
            });
        }

        public static TestServerFixture<TStartup> Create(string environment = EnvironmentNames.Integration)
        {
            return new TestServerFixture<TStartup>(new TestServerOptions
            {
                Environment = environment
            });
        }

        public static TestServerFixture<TStartup> Create(Action<IServiceCollection> configureServices,
            string environment = EnvironmentNames.Integration)
        {
            return new TestServerFixture<TStartup>(new TestServerOptions
            {
                Environment = environment,
                ConfigureServices = configureServices
            });
        }
    }
}
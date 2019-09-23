using System;
using System.IO;
using EasyApi.AspNetCore.Logging.SeriLog.Fluent;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace EasyApi.AspNetCore
{
    public static class WebHost
    {
        private static IWebHost Build<TStartup>(Func<ILoggerBuilder, IBuildLogger> loggerBuilderFactory, string[] args)
            where TStartup : class
        {
            if (loggerBuilderFactory == null) throw new ArgumentNullException(nameof(loggerBuilderFactory));

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = context.HostingEnvironment;

                    builder.SetBasePath(env.ContentRootPath)
                        .AddJsonFile("hosting.json", true)
                        .AddJsonFile("appsettings.json", false, true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                        .AddEnvironmentVariables()
                        .AddCommandLine(args);
                })
                .UseIISIntegration()
                .UseStartup<TStartup>()
                .UseSerilog((context, configuration) =>
                {
                    var loggerBuilder = loggerBuilderFactory(new LoggerBuilder(context, configuration));
                    loggerBuilder.Build<TStartup>();
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
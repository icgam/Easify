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
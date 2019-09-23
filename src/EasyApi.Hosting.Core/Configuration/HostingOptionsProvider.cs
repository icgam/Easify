using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace EasyApi.Hosting.Core.Configuration
{
    public sealed class HostingOptionsProvider
    {
        private const string InteractiveFlag = "--interactive";

        public HostingOptions GetHostingOptions(string[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var isService = LaunchedAsService(args);
            var pathToContentRoot = GetPathToContentRoot(isService);
            var configuration = LoadConfiguration(pathToContentRoot, args);

            return new HostingOptions(pathToContentRoot, isService, configuration);
        }

        private IConfiguration LoadConfiguration(string pathToContentRoot, string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(pathToContentRoot, "appSettings.json"), false, true)
                .AddJsonFile(Path.Combine(pathToContentRoot, $"appsettings.{environmentName}.json"), true, true)
                .AddCommandLine(args)
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            return configuration;
        }

        private bool LaunchedAsService(string[] args)
        {
            return (Debugger.IsAttached || args.Contains(InteractiveFlag)) == false;
        }

        private string GetPathToContentRoot(bool isService)
        {
            var pathToContentRoot = Directory.GetCurrentDirectory();
            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                pathToContentRoot = Path.GetDirectoryName(pathToExe);
            }
            return pathToContentRoot;
        }
    }
}
using System;
using EasyApi.Logging.SeriLog;
using Microsoft.AspNetCore.Hosting;
using Serilog.Configuration;

namespace EasyApi.AspNetCore.Logging.SeriLog
{
    public sealed class LoggerConfigurationServices : ILoggerConfiguration
    {
        public LoggerSinkConfiguration SinkConfiguration { get; }
        public string ApplicationName => Environment.ApplicationName;
        public string EnvironmentName => Environment.EnvironmentName;

        public IHostingEnvironment Environment { get; }

        public LoggerConfigurationServices(LoggerSinkConfiguration sinkConfiguration, IHostingEnvironment environment)
        {
            SinkConfiguration = sinkConfiguration ?? throw new ArgumentNullException(nameof(sinkConfiguration));
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }
    }
}
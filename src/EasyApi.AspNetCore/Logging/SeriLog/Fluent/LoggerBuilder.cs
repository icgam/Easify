using System;
using System.IO;
using System.Reflection;
using EasyApi.Configurations;
using EasyApi.Logging.SeriLog;
using EasyApi.Logging.SeriLog.OptionsBuilder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace EasyApi.AspNetCore.Logging.SeriLog.Fluent
{
    // TODO: The logging should be revamped. It's highly dependent to Asp.NEt Core which it shouldn't be
    // TODO: AppProfile need to be seen as part of this process
    // TODO: There should be a way to extend the variables without having them statically in the process 
    public sealed class LoggerBuilder : ILoggerBuilder, IBuildLogger
    {
        private const string LogMessageTemplate =
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{MachineName}] [{EnvironmentUserName}] [{ProcessId}] " +
            "[{UserName}] [{CorrelationId}] [{ThreadId}] [{Level}] {Message}{NewLine}{Exception}";

        private const string SerilogConfigSectionName = "Logging:Serilog";
        private const string SerilogMinimumLevelKey = "MinimumLevel";
        private const LogEventLevel DefaultLogLevel = LogEventLevel.Information;
        private readonly LoggerConfiguration _configuration;
        private readonly WebHostBuilderContext _context;

        private Func<ISetFileSizeLimit, IBuildSeriLogOptions> _optionsProvider;
        private Func<ILoggerConfiguration, IBuildSink> _sinksProvider;

        public LoggerBuilder(WebHostBuilderContext context, LoggerConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Build<TStartup>() where TStartup : class
        {
            ConfigureLogger<TStartup>(_context, _configuration, _optionsProvider, _sinksProvider);
        }

        public IConfiguration Configuration => _context.Configuration;
        public IHostingEnvironment Environment => _context.HostingEnvironment;

        public IBuildLogger ConfigureLogger<TStartup>() where TStartup : class
        {
            _sinksProvider = null;
            _optionsProvider = provider => provider;

            return this;
        }

        public IBuildLogger ConfigureLogger<TStartup>(Func<ILoggerConfiguration, IBuildSink> sinksProvider)
            where TStartup : class
        {
            _sinksProvider = sinksProvider;
            _optionsProvider = provider => provider;

            return this;
        }

        public IBuildLogger ConfigureLogger<TStartup>(Func<ISetFileSizeLimit, IBuildSeriLogOptions> optionsProvider,
            Func<ILoggerConfiguration, IBuildSink> sinksProvider) where TStartup : class
        {
            if (optionsProvider == null) throw new ArgumentNullException(nameof(optionsProvider));
            _sinksProvider = sinksProvider;
            _optionsProvider = provider => provider;

            return this;
        }

        public IBuildLogger ConfigureLogger<TStartup>(Func<ISetFileSizeLimit, IBuildSeriLogOptions> optionsProvider)
            where TStartup : class
        {
            _optionsProvider = optionsProvider ?? throw new ArgumentNullException(nameof(optionsProvider));
            _sinksProvider = null;

            return this;
        }

        private void ConfigureLogger<TStartup>(WebHostBuilderContext context, LoggerConfiguration loggerConfiguration,
            Func<ISetFileSizeLimit, IBuildSeriLogOptions> optionsProvider,
            Func<ILoggerConfiguration, IBuildSink> sinksProvider) where TStartup : class
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (loggerConfiguration == null) throw new ArgumentNullException(nameof(loggerConfiguration));

            var env = context.HostingEnvironment;
            var configuration = context.Configuration;
            var applicationInfo = configuration.GetApplicationInfo();

            var optionsBuilder = new SeriLogOptionsBuilder();
            var options = optionsProvider(optionsBuilder).Build();
            var assemblyName = typeof(TStartup).GetTypeInfo().Assembly.GetName().Name;
            var environmentName = applicationInfo.Environment ?? env.EnvironmentName;
            var logFilePath = GetLogFilePath(env, options);

            loggerConfiguration
                .MinimumLevel.ControlledBy(LoggingLevelSwitchProvider.Instance)
                .IgnoreSystemLogs()
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithApplicationName(assemblyName)
                .Enrich.WithEnvironmentName(environmentName)
                .WriteTo.RollingFile(Path.Combine(logFilePath, $"{assemblyName}-{environmentName}-{{Date}}.log"),
                    outputTemplate: LogMessageTemplate,
                    retainedFileCountLimit: options.LogFilesToRetain,
                    fileSizeLimitBytes: options.LogFileSizeLimitInBytes,
                    flushToDiskInterval: options.FlushToDiskInterval)
                .WriteTo.LiterateConsole()
                .WriteTo.InMemoryCache();

            LoggingLevelSwitchProvider.Instance.MinimumLevel =
                GetMinimumLogLevelOrUseDefault(configuration, DefaultLogLevel);

            sinksProvider?.Invoke(new LoggerConfigurationServices(loggerConfiguration.WriteTo, env)).Build();
        }

        private static string GetLogFilePath(IHostingEnvironment env, LoggingOptions options)
        {
            var defaultPath = env.IsDevelopment() ? $"{env.ContentRootPath}\\logs" : "D:\\logs";
            return options.LogsPathSet ? options.LogsPath : defaultPath;
        }

        private static LogEventLevel GetMinimumLogLevelOrUseDefault(IConfiguration configuration,
            LogEventLevel defaultLogEventLevel)
        {
            try
            {
                var config = configuration.GetSection(SerilogConfigSectionName);
                var minimumLogLevelToParse = config[SerilogMinimumLevelKey];
                return (LogEventLevel) Enum.Parse(typeof(LogEventLevel), minimumLogLevelToParse);
            }
            catch (ArgumentException)
            {
                return defaultLogEventLevel;
            }
        }
    }
}
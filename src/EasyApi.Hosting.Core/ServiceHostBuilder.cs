using System;
using EasyApi.AspNetCore.Logging.SeriLog.Fluent;
using EasyApi.Hosting.Core.Configuration;
using EasyApi.Hosting.Core.HostContainer;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace EasyApi.Hosting.Core
{
    //TODO: This should be revised regarding to the new capabilities in v2.0
    public sealed class ServiceHostBuilder<TStartup> where TStartup : class
    {
        private readonly Func<IWebHost, IServiceHost> _serviceContainerBuilder;
        private readonly HostingOptions _hostingOptions;

        public ServiceHostBuilder(HostingOptions hostingOptions, Func<IWebHost, IServiceHost> serviceContainerBuilder)
        {
            _serviceContainerBuilder = serviceContainerBuilder ?? throw new ArgumentNullException(nameof(serviceContainerBuilder));
            _hostingOptions = hostingOptions ?? throw new ArgumentNullException(nameof(hostingOptions));
        }

        public IServiceHost Build(Func<ILoggerBuilder, IBuildLogger> loggerBuilderFactory, string[] args)
        {
            var webHost = new WebHostBuilder()
               .UseKestrel()
               .UseContentRoot(_hostingOptions.PathToContentRoot)
               .UseUrls(_hostingOptions.BaseUrl)
               .UseConfiguration(_hostingOptions.Configuration)
               .UseStartup<TStartup>()
               .UseSerilog((context, configuration) =>
                {
                    var loggerBuilder = loggerBuilderFactory(new LoggerBuilder(context, configuration));
                    loggerBuilder.Build<TStartup>();
                })
               .Build();
            
            if (_hostingOptions.LaunchedAsService)
                return _serviceContainerBuilder(webHost);
                 
            return new WebHostContainer(webHost);
        }
    }
}
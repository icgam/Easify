using System;
using EasyApi.AspNetCore.Logging.SeriLog;
using EasyApi.Logging;
using Foil;
using Foil.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;

namespace EasyApi.AspNetCore.UnitTests.Helpers
{
    public class ServiceCollectionFixture
    {
        public InMemoryLogger<LogInterceptor> Log { get; private set; }

        public IServiceProvider BuildProvider(Func<IServiceCollection, IServiceCollection> serviceConfigurator,
            Func<LogLevel, bool> logLevelFilter)
        {
            if (serviceConfigurator == null) throw new ArgumentNullException(nameof(serviceConfigurator));
            if (logLevelFilter == null) throw new ArgumentNullException(nameof(logLevelFilter));

            var services = new ServiceCollection();

            Log = new InMemoryLogger<LogInterceptor>(typeof(ILogger).Name, (s, level) => logLevelFilter(level));
            services.AddTransient<IArgumentsFormatter, ArgumentsFormatter>();
            services.AddSingleton(p => new ArgumentFormatterOptions
            {
                Formatting = Formatting.Indented
            });
            services.AddSingleton(p => Log);
            services.AddSingleton<ILogger<LogInterceptor>>(p => Log);
            services.AddSingleton(p => Substitute.For<ILogger<ArgumentsFormatter>>());
            serviceConfigurator(services);

            return services.BuildServiceProvider();
        }

        public IServiceProvider BuildProvider(Func<LogLevel, bool> logLevelFilter)
        {
            return BuildProvider(s =>
                    s.AddTransientWithInterception<IFakeService, FakeService>(m =>
                        m.InterceptBy<LogInterceptor>().UseMethodConvention<NonQueryMethodsConvention>()),
                logLevelFilter);
        }
    }
}
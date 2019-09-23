using System;
using EasyApi.Bootstrap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyApi.AspNetCore.Bootstrap
{
    public static class StartupExtensions
    {
        public static IServiceProvider BootstrapApp<TStartup>(this IServiceCollection services,
            IConfiguration configuration,
            Func<IConfigureApplicationBootstrapper, IBootstrapApplication> appBootstrapperProvider
        )
            where TStartup : class
        {
            var bootstrapper = appBootstrapperProvider(new AppBootstrapper<TStartup>(services, configuration));
            return bootstrapper.Bootstrap();
        }
        
        public static IBootstrapApplication
            AddServices(this IConfigureContainer bootstrapper,
                Action<IServiceCollection, IConfiguration> serviceConfigurationProvider)
        {
            if (bootstrapper == null) throw new ArgumentNullException(nameof(bootstrapper));
            if (serviceConfigurationProvider == null)
                throw new ArgumentNullException(nameof(serviceConfigurationProvider));

            var factory = new NativeContainerFactory(serviceConfigurationProvider);
            return bootstrapper.UseContainer(factory);
        }

        public static IBootstrapApplication
            AddServices<TContainer>(
                this Func<Action<TContainer, IConfiguration>, IBootstrapApplication>
                    bootstrapperServicesConfigurator,
                Action<TContainer, IConfiguration> serviceConfigurationProvider)
        {
            if (bootstrapperServicesConfigurator == null)
                throw new ArgumentNullException(nameof(bootstrapperServicesConfigurator));
            if (serviceConfigurationProvider == null)
                throw new ArgumentNullException(nameof(serviceConfigurationProvider));

            return bootstrapperServicesConfigurator(serviceConfigurationProvider);
        }
    }
}
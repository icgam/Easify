using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyApi.Bootstrap
{
    public abstract class ContainerFactory<TContainer> where TContainer : class
    {
        private readonly Action<TContainer, IConfiguration> _serviceConfigurationProvider;

        protected ContainerFactory(Action<TContainer, IConfiguration> serviceConfigurationProvider)
        {
            _serviceConfigurationProvider = serviceConfigurationProvider ?? throw new ArgumentNullException(nameof(serviceConfigurationProvider));
        }

        protected abstract IServiceProvider ConfigureContainer(
            Action<TContainer, IConfiguration> serviceConfigurationProvider,
            IServiceCollection services,
            IConfiguration configuration);

        public IServiceProvider Create(IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            return ConfigureContainer(_serviceConfigurationProvider, services, configuration);
        }
    }
}
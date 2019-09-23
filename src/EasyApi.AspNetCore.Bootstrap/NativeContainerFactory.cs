using System;
using EasyApi.Bootstrap;
using EasyApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyApi.AspNetCore.Bootstrap
{
    // TODO: Should be reverse dependency to the system. Using container in host registration rather than startup
    public sealed class
        NativeContainerFactory : ContainerFactory<IServiceCollection>
    {
        public NativeContainerFactory(
            Action<IServiceCollection, IConfiguration> serviceConfigurationProvider) : base(
            serviceConfigurationProvider)
        {
        }

        protected override IServiceProvider ConfigureContainer(
            Action<IServiceCollection, IConfiguration> serviceConfigurationProvider,
            IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IComponentResolver, ServiceCollectionBasedComponentResolver>();
            serviceConfigurationProvider(services, configuration);
            return services.BuildServiceProvider();
        }
    }
}
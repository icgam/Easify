using System;
using System.Collections.Generic;
using EasyApi.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace EasyApi
{
    // TODO: Should be renamed
    public sealed class ServiceCollectionBasedComponentResolver : IComponentResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceCollectionBasedComponentResolver(IServiceProvider  serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public bool IsRegistered<TComponent>() where TComponent : class
        {
            return IsRegistered(typeof(TComponent));
        }

        public bool IsRegistered(Type type)
        {
            return _serviceProvider.GetService(type) != null;
        }

        public IEnumerable<TComponent> Resolve<TComponent>() where TComponent : class
        {
            return _serviceProvider.GetServices<TComponent>();
        }

        public IEnumerable<object> Resolve(Type type)
        {
            return _serviceProvider.GetServices(type);
        }
    }
}
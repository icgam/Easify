using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace EasyApi.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ReplaceFirst<TService, TImplementation>(this IServiceCollection services,
            ServiceLifetime lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(TService));
            if (descriptorToRemove != null)
                services.Remove(descriptorToRemove);

            var descriptorToAdd = new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime);
            services.Add(descriptorToAdd);

            return services;
        }

        public static IServiceCollection RemoveFirstOrNothing<TService>(this IServiceCollection services)
            where TService : class
        {
            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(TService));
            if (descriptorToRemove != null)
                services.Remove(descriptorToRemove);

            return services;
        }
    }
}
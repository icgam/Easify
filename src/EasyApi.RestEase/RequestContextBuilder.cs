using System;

namespace EasyApi.RestEase
{
    public sealed class RequestContextBuilder<TContainer> where TContainer : class
    {
        private RequestContextBuilder(TContainer container)
        {
            Container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public TContainer Container { get; }

        public static RequestContextBuilder<TServicesContainer> For<TServicesContainer>(TServicesContainer container)
            where TServicesContainer : class
        {
            return new RequestContextBuilder<TServicesContainer>(container);
        }
    }
}
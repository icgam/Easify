using System;
using System.Linq;
using AutoMapper;
using EasyApi.Extensions;

namespace EasyApi
{
    public sealed class DefaultTypeMapper : ITypeMapper
    {
        private readonly IMapper _mapper;

        public DefaultTypeMapper(Action<IMapperConfigurationExpression> mappingsConfiguration,
            IComponentResolver container)
        {
            if (mappingsConfiguration == null) throw new ArgumentNullException(nameof(mappingsConfiguration));
            if (container == null) throw new ArgumentNullException(nameof(container));

            var config = new MapperConfiguration(cfg =>
            {
                cfg.ConstructServicesUsing(service => container.Resolve(service).SingleOrDefault());
                mappingsConfiguration(cfg);
            });

            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }
    }
}
// This software is part of the Easify framework
// Copyright (C) 2019 Intermediate Capital Group
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Linq;
using AutoMapper;

namespace Easify
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
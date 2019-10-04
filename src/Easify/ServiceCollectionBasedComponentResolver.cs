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
using System.Collections.Generic;
using Easify.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Easify
{
    // TODO: Should be renamed
    public sealed class ServiceCollectionBasedComponentResolver : IComponentResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceCollectionBasedComponentResolver(IServiceProvider serviceProvider)
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
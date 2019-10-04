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
 using Easify.Bootstrap;
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
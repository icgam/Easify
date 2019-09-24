// This software is part of the EasyApi framework
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyApi.Configurations.Fluents
{
    public sealed class ConfigurationSectionBuilder : IAddFirstSection, IAddExtraConfigSection
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceCollection _services;

        public ConfigurationSectionBuilder(IServiceCollection services, IConfiguration configuration)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IAddExtraConfigSection And<TSection>() where TSection : class, new()
        {
            var sectionName = typeof(TSection).Name;
            _services.Configure<TSection>(_configuration.GetSection(sectionName));

            return this;
        }

        public IAddExtraConfigSection And<TSection>(string section) where TSection : class, new()
        {
            if (string.IsNullOrWhiteSpace(section))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(section));
            _services.Configure<TSection>(_configuration.GetSection(section));
            return this;
        }

        public IAddExtraConfigSection AddSection<TSection>()
            where TSection : class, new()
        {
            _services.AddOptions();

            var builder = new ConfigurationSectionBuilder(_services, _configuration);
            return builder.And<TSection>();
        }

        public IAddExtraConfigSection AddSection<TSection>(string section) where TSection : class, new()
        {
            if (string.IsNullOrWhiteSpace(section))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(section));

            _services.Configure<TSection>(_configuration.GetSection(section));
            return this;
        }

        public void Build()
        {
            AddSection<Application>();
        }
    }
}
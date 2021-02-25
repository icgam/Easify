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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Easify.Configurations.Fluents
{
    public sealed class ConfigurationOptionBuilder : IAddFirstSection, IAddExtraConfigSection
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;

        public ConfigurationOptionBuilder(IServiceCollection services, IConfiguration configuration)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IAddExtraConfigSection And<TSection>() where TSection : class, new()
        {
            return AddOptions<TSection>(GetSectionName<TSection>());
        }

        public IAddExtraConfigSection And<TSection>(string section) where TSection : class, new()
        {
            return AddOptions<TSection>(section);
        }

        public IAddExtraConfigSection AddSection<TSection>()
            where TSection : class, new()
        {
            return AddOptions<TSection>(GetSectionName<TSection>());
        }

        public IAddExtraConfigSection AddSection<TSection>(string section) where TSection : class, new()
        {
            return AddOptions<TSection>(section);
        }

        public void Build()
        {
            AddSection<Application>();
        }

        private static string GetSectionName<TSection>() where TSection : class, new()
        {
            return typeof(TSection).Name;
        }

        private ConfigurationOptionBuilder AddOptions<TSection>(string section) where TSection : class, new()
        {
            if (string.IsNullOrWhiteSpace(section))
                throw new ArgumentException("Section name cannot be null or whitespace.", nameof(section));

            _services.AddOptions<TSection>().Bind(_configuration.GetSection(section)).ValidateDataAnnotations();
            return this;
        }
    }
}
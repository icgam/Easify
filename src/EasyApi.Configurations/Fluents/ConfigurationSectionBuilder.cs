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
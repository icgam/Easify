using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace EasyApi.AspNetCore.Serializations
{
    // TODO: Should be moved outside (Higher level component)
    public static class JsonSerializerSettingsExtensions
    {
        public static JsonSerializerSettings ConfigureJsonSettings(this JsonSerializerSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Converters.Add(new StringEnumConverter());

            return settings;
        }
    }
}
using Newtonsoft.Json;

namespace EasyApi.Logging
{
    public sealed class ArgumentFormatterOptions
    {
        public Formatting Formatting { get; set; } = Formatting.Indented;
    }
}
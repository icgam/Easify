using System;

namespace Easify.Notification.Templating
{
    public sealed class TemplateDefinition
    {
        public TemplateDefinition(string name, string template)
        {
            if (string.IsNullOrWhiteSpace(template))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(template));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

            Name = name;
            Template = template;
        }

        public string Name { get; }
        public string Template { get; }
    }
}
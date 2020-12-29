using System;

namespace Easify.Notification.Templating
{
    public sealed class RenderedTemplate<T> where T : class
    {
        public RenderedTemplate(string content, T data)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(content));

            Data = data ?? throw new ArgumentNullException(nameof(data));
            Content = content;
        }

        public string Content { get; }
        public T Data { get; }
    }
}
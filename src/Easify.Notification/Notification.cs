using System;

namespace Easify.Notification
{
    public class Notification<T> where T : class
    {
        public Notification(string title, string templateName, T data)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            TemplateName = templateName ?? throw new ArgumentNullException(nameof(templateName));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public string Title { get; }
        public string TemplateName { get; }
        public T Data { get; }

        public Notification<T> From(string title, string templateName, T data)
        {
            return new Notification<T>(title, templateName, data);
        }
    }
}
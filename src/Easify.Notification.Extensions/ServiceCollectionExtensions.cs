using System;
using Easify.Notification.Configuration;
using Easify.Notification.Messaging;
using Easify.Notification.Templating;
using Easify.Notification.Templating.HandleBars;
using Microsoft.Extensions.DependencyInjection;

namespace Easify.Notification.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNotification(this IServiceCollection services,
            Action<NotificationOptions> configureNotificationOptions = null,
            Action<SmtpOptions> configureSmtpOptions = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddOptions()
                .Configure<NotificationOptions>(options => configureNotificationOptions?.Invoke(options));
            services.AddOptions().Configure<SmtpOptions>(options => configureSmtpOptions?.Invoke(options));

            services.AddTransient<ITemplateProvider, FileBasedTemplateProvider>();
            services.AddTransient<IMessagingService, MailKitMessagingService>();
            services.AddTransient<ITemplateContentRenderer, HandleBarsTemplateContentRenderer>();
            services.AddTransient<ITemplateRenderer, TemplateRenderer>();
            services.AddTransient<INotificationService, NotificationService>();

            return services;
        }
    }
}
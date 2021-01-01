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
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
using System.Linq;
using System.Threading.Tasks;
using Easify.Notification.Configuration;
using Easify.Notification.Configuration.Validators;
using Easify.Notification.Exceptions;
using Easify.Notification.Messaging;
using Easify.Notification.Templating;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Easify.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IMessagingService _messagingService;
        private readonly NotificationOptions _options;
        private readonly ITemplateRenderer _templateRenderer;
        private readonly ILogger<NotificationService> _logger;

        // TODO: Think on moving this to a dependent service 
        private readonly NotificationOptionsValidator _validator = new NotificationOptionsValidator();

        public NotificationService(
            IMessagingService messagingService, 
            ITemplateRenderer templateRenderer,
            IOptions<NotificationOptions> optionAccessor,
            ILogger<NotificationService> logger)
        {
            _messagingService = messagingService ?? throw new ArgumentNullException(nameof(messagingService));
            _templateRenderer = templateRenderer ?? throw new ArgumentNullException(nameof(templateRenderer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = optionAccessor?.Value ?? throw new ArgumentNullException(nameof(optionAccessor));
        }

        public async Task SendNotificationAsync<T>(Notification<T> notification,
            string profileName = NotificationProfileNames.DefaultProfile) where T : class
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));
            if (profileName == null) throw new ArgumentNullException(nameof(profileName));

            EnsureOptions();

            var template = ResolveTemplate(notification.TemplateName);
            var rendered = await RenderTemplate(notification.Data, template);

            var message = GenerateMessage(profileName, notification.Title, rendered.Content);
            await SendMessageAsync(message);
        }

        private void EnsureOptions()
        {
            var result = _validator.Validate(_options);
            if (result.IsValid)
                return;

            _logger.LogError($"Invalid format for NotificationOptions. Check the configuration");
            throw NotificationOptionsException.FromValidationResults(result);
        }

        private Task SendMessageAsync(Message message)
        {
            return _messagingService.SendAsync(message);
        }

        private async Task<RenderedTemplate<T>> RenderTemplate<T>(T data, NotificationTemplate template) where T : class
        {
            return await _templateRenderer.RenderTemplateAsync(template.Name, template.Path, data);
        }

        private Message GenerateMessage(string profileName, string title, string content)
        {
            _logger.LogInformation($"Generate message from profile {profileName}");

            var profile = _options.Profiles.FirstOrDefault(m =>
                m.ProfileName.Equals(profileName, StringComparison.CurrentCultureIgnoreCase));

            if (profile == null)
            {
                _logger.LogError($"Can't find profile from configuration with {profileName}");
                throw new NotificationProfileNotFoundException(profileName);
            }

            var sender = _options.Sender.ToEmailAddress();
            var recipients = profile.Audiences.Select(m => m.Email.ToEmailAddress());
            var message = Message.From(title, content, sender, recipients);

            return message;
        }

        private NotificationTemplate ResolveTemplate(string templateName)
        {
            _logger.LogInformation($"Resolve template from profiles in configuration {templateName}");

            var template = _options.Templates.FirstOrDefault(m =>
                m.Name.Equals(templateName, StringComparison.CurrentCultureIgnoreCase));

            if (template != null)
                return template;

            _logger.LogError($"Error in resolving template from configuration with {templateName}");
            throw new NotificationTemplateNotFoundException(templateName);
        }
    }
}
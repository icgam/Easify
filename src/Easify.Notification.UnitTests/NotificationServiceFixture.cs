using System;
using System.Threading.Tasks;
using ICG.Core.Notifications.Configuration;
using ICG.Core.Notifications.Messaging;
using ICG.Core.Templating;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace ICG.Core.Notifications.UnitTests
{
    public class NotificationServiceFixture
    {
        public NotificationServiceFixture()
        {
            var expectedContent = "SampleContent";
            ExpectedModel = new Model();

            ValidOptionAccessor = Substitute.For<IOptions<NotificationOptions>>();
            ValidOptionAccessor.Value.Returns(ValidOption);

            InvalidOptionAccessor = Substitute.For<IOptions<NotificationOptions>>();
            InvalidOptionAccessor.Value.Returns(InvalidOption);

            TemplateRenderer = Substitute.For<ITemplateRenderer>();
            TemplateRenderer.RenderTemplateAsync(Arg.Any<string>(), Arg.Any<string>(), ExpectedModel)
                .Returns(Task.FromResult(new RenderedTemplate<Model>(expectedContent, ExpectedModel)));

            MessagingService = Substitute.For<IMessagingService>();
        }

        public IMessagingService MessagingService { get; }

        public IOptions<NotificationOptions> ValidOptionAccessor { get; }

        public IOptions<NotificationOptions> InvalidOptionAccessor { get; }

        public ILogger<NotificationService> Logger => Substitute.For<ILogger<NotificationService>>();

        public string ValidTemplate = "Template1";
        public string ValidProfile = "DefaultProfile";

        public ITemplateRenderer TemplateRenderer { get; }

        public Model ExpectedModel { get; }

        private NotificationOptions ValidOption => new NotificationOptions
        {
            Sender = "sender@icgam.com",
            Profiles = new[]
            {
                new NotificationProfile
                {
                    ProfileName = "DefaultProfile",
                    Audiences = new[]{ new NotificationAudience {Email = "mohammad.moattar@icgam.com" } }
                },
            },
            Templates = new[]
            {
                new NotificationTemplate
                {
                    Name = "Template1",
                    Path = "Template1Path"
                }
            }
        };
        private NotificationOptions InvalidOption => new NotificationOptions
        {
            Profiles = new[]
            {
                new NotificationProfile
                {
                    Audiences = new[]{ new NotificationAudience {Email = "mohammad.moattar@icgam.com" } }
                },
            },
            Templates = new[]
            {
                new NotificationTemplate
                {
                    Path = "Template1Path"
                }
            }
        };

        public class Model
        {
            public DateTime AsOf { get; set; } = new DateTime(2018, 9, 21, 4, 30, 30);
            public string Title { get; set; } = "RenderAsync";
        }
    }
}
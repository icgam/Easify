using System;
using System.Threading.Tasks;
using FluentAssertions;
using ICG.Core.Notifications.Exceptions;
using ICG.Core.Notifications.Messaging;
using NSubstitute;
using Xunit;

namespace ICG.Core.Notifications.UnitTests
{
    public class NotificationServiceTests : IClassFixture<NotificationServiceFixture>
    {
        private readonly NotificationServiceFixture _fixture;

        public NotificationServiceTests(NotificationServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_SendNotificationAsync_IsSuccessfulWhenTheProfileAndTemplateAreCorrect()
        {
            // Given
            var messagingService = _fixture.MessagingService;
            var sut = new NotificationService(messagingService, _fixture.TemplateRenderer, _fixture.ValidOptionAccessor, _fixture.Logger);
            var notification = new Notification<NotificationServiceFixture.Model>("Title", _fixture.ValidTemplate, _fixture.ExpectedModel);

            // When
            await sut.SendNotificationAsync(notification, _fixture.ValidProfile);

            // Then
            await messagingService.Received(1).SendAsync(Arg.Is<Message>(m => IsValidMessage(m)));
        }

        private bool IsValidMessage(Message message)
        {
            message.Subject.Should().Be("Title");
            message.Content.Should().Be("SampleContent");
            message.Sender.Email.Should().Be("sender@icgam.com");
            message.Recipients.Should().HaveCount(1);

            return true;
        }

        [Fact]
        public void Should_SendNotificationAsync_ThrowTheRigthExcetionWhenTheProfileIsNotAvailable()
        {
            // Given
            var messagingService = _fixture.MessagingService;
            var sut = new NotificationService(messagingService, _fixture.TemplateRenderer, _fixture.ValidOptionAccessor, _fixture.Logger);
            var notification = new Notification<NotificationServiceFixture.Model>("Title", _fixture.ValidTemplate, _fixture.ExpectedModel);

            // When
            Func<Task> func = async () => await sut.SendNotificationAsync(notification, "InvalidProfile");

            // Then
            func.Should().Throw<NotificationProfileNotFoundException>();
        }

        [Fact]
        public void Should_SendNotificationAsync_ThrowTheRigthExcetionWhenTheNotificationOptionsIsInvalid()
        {
            // Given
            var sut = new NotificationService(_fixture.MessagingService, _fixture.TemplateRenderer, _fixture.InvalidOptionAccessor, _fixture.Logger);
            var notification = new Notification<NotificationServiceFixture.Model>("Title", _fixture.ValidTemplate, _fixture.ExpectedModel);

            // When
            Func<Task> func = async () => await sut.SendNotificationAsync(notification);

            // Then
            func.Should().Throw<NotificationOptionsException>()
                .And.Errors.Should().HaveCount(3)
                .And.Contain(m =>
                    m.ErrorMessage == "'Name' should not be empty." && m.PropertyName == "Templates[0].Name")
                .And.Contain(m =>
                    m.ErrorMessage == "'Sender' should not be empty." && m.PropertyName == "Sender")
                .And.Contain(m =>
                    m.ErrorMessage == "'Profile Name' should not be empty." &&
                    m.PropertyName == "Profiles[0].ProfileName");
        }

        [Fact]
        public void Should_SendNotificationAsync_ThrowTheRigthExcetionWhenTheTemplateIsNotAvailable()
        {
            // Given
            var messagingService = _fixture.MessagingService;
            var sut = new NotificationService(messagingService, _fixture.TemplateRenderer, _fixture.ValidOptionAccessor, _fixture.Logger);
            var notification = new Notification<NotificationServiceFixture.Model>("Title", "InvalidTemplate", _fixture.ExpectedModel);

            // When
            Func<Task> func = async () => await sut.SendNotificationAsync(notification, _fixture.ValidProfile);

            // Then
            func.Should().Throw<NotificationTemplateNotFoundException>();
        }
    }
}
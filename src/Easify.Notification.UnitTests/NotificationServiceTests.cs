using System;
using System.Threading.Tasks;
using Easify.Notification.Exceptions;
using Easify.Notification.Messaging;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Easify.Notification.UnitTests
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
        public async Task Should_SendNotificationAsync_ThrowTheRightExceptionWhenTheProfileIsNotAvailable()
        {
            // Given
            var messagingService = _fixture.MessagingService;
            var sut = new NotificationService(messagingService, _fixture.TemplateRenderer, _fixture.ValidOptionAccessor, _fixture.Logger);
            var notification = new Notification<NotificationServiceFixture.Model>("Title", _fixture.ValidTemplate, _fixture.ExpectedModel);

            // When
            // Then
            await Assert.ThrowsAsync<NotificationProfileNotFoundException>(
                () => sut.SendNotificationAsync(notification, "InvalidProfile"));
        }

        [Fact]
        public async Task Should_SendNotificationAsync_ThrowTheRightExceptionWhenTheNotificationOptionsIsInvalid()
        {
            // Given
            var sut = new NotificationService(_fixture.MessagingService, _fixture.TemplateRenderer,
                _fixture.InvalidOptionAccessor, _fixture.Logger);
            var notification =
                new Notification<NotificationServiceFixture.Model>("Title", _fixture.ValidTemplate,
                    _fixture.ExpectedModel);
            
            // When
            // Then
            var exception = await Assert.ThrowsAsync<NotificationOptionsException>(
                () => sut.SendNotificationAsync(notification));
            
            Assert.Collection(exception.Errors,
                e =>
                {
                    Assert.Contains("'Sender' must not be empty.", e.ErrorMessage);
                    Assert.Equal("Sender", e.PropertyName);
                },
                e =>
                {
                    Assert.Contains("'Profile Name' must not be empty.", e.ErrorMessage);
                    Assert.Equal("Profiles[0].ProfileName", e.PropertyName);
                },
                e =>
                {
                    Assert.Contains("'Name' must not be empty.", e.ErrorMessage);
                    Assert.Equal("Templates[0].Name", e.PropertyName);
                });
        }

        [Fact]
        public void Should_SendNotificationAsync_ThrowTheRightExceptionWhenTheTemplateIsNotAvailable()
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
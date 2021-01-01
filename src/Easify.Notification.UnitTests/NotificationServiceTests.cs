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
        public async Task GivenProfileAndTemplate_WhenCallingSendNotificationAsync_ShouldBeSuccessful()
        {
            // Arrange
            var messagingService = _fixture.MessagingService;
            var sut = new NotificationService(messagingService, _fixture.TemplateRenderer, _fixture.ValidOptionAccessor, _fixture.Logger);
            var notification = new Notification<NotificationServiceFixture.Model>("Title", _fixture.ValidTemplate, _fixture.ExpectedModel);

            // Act
            await sut.SendNotificationAsync(notification, _fixture.ValidProfile);

            // Assert
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
        public async Task GivenInvalidProfile_WhenCallingSendNotificationAsync_ShouldThrowNotificationProfileNotFoundException()
        {
            // Arrange
            var messagingService = _fixture.MessagingService;
            var sut = new NotificationService(messagingService, _fixture.TemplateRenderer, _fixture.ValidOptionAccessor, _fixture.Logger);
            var notification = new Notification<NotificationServiceFixture.Model>("Title", _fixture.ValidTemplate, _fixture.ExpectedModel);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotificationProfileNotFoundException>(
                () => sut.SendNotificationAsync(notification, "InvalidProfile"));
        }

        [Fact]
        public async Task GivenInvalidNotificationOptions_WhenCallingSendNotificationAsync_NotificationOptionsException()
        {
            // Arrange
            var sut = new NotificationService(_fixture.MessagingService, _fixture.TemplateRenderer,
                _fixture.InvalidOptionAccessor, _fixture.Logger);
            var notification =
                new Notification<NotificationServiceFixture.Model>("Title", _fixture.ValidTemplate,
                    _fixture.ExpectedModel);
            
            // Act
            // Assert
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
        public void GivenInvalidTemplate_WhenCallingSendNotificationAsync_NotificationTemplateNotFoundException()
        {
            // Arrange
            var messagingService = _fixture.MessagingService;
            var sut = new NotificationService(messagingService, _fixture.TemplateRenderer, _fixture.ValidOptionAccessor, _fixture.Logger);
            var notification = new Notification<NotificationServiceFixture.Model>("Title", "InvalidTemplate", _fixture.ExpectedModel);

            // Act
            Func<Task> func = async () => await sut.SendNotificationAsync(notification, _fixture.ValidProfile);

            // Assert
            func.Should().Throw<NotificationTemplateNotFoundException>();
        }
    }
}
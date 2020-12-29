using FluentValidation;

namespace Easify.Notification.Configuration.Validators
{
    public class NotificationAudienceValidator : AbstractValidator<NotificationAudience>
    {
        public NotificationAudienceValidator()
        {
            RuleFor(m => m.Email).NotEmpty().EmailAddress();
        }
    }
}
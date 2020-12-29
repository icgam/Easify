using FluentValidation;

namespace Easify.Notification.Configuration.Validators
{
    public class NotificationProfileValidator : AbstractValidator<NotificationProfile>
    {
        public NotificationProfileValidator()
        {
            RuleFor(m => m.ProfileName).NotEmpty();
            RuleForEach(m => m.Audiences).NotEmpty().SetValidator(new NotificationAudienceValidator());
        }
    }
}
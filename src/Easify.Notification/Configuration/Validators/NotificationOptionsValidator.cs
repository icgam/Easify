using FluentValidation;

namespace Easify.Notification.Configuration.Validators
{
    public class NotificationOptionsValidator : AbstractValidator<NotificationOptions>
    {
        public NotificationOptionsValidator()
        {
            RuleFor(m => m.Sender).NotEmpty();
            RuleForEach(m => m.Profiles).NotEmpty().SetValidator(new NotificationProfileValidator());
            RuleForEach(m => m.Templates).NotEmpty().SetValidator(new NotificationTemplateValidator());
        }
    }
}
using FluentValidation;

namespace Easify.Notification.Configuration.Validators
{
    public class NotificationTemplateValidator : AbstractValidator<NotificationTemplate>
    {
        public NotificationTemplateValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.Path).NotEmpty();
        }
    }
}
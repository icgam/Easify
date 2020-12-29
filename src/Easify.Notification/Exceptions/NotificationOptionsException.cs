using System;
using System.Linq;
using FluentValidation.Results;

namespace Easify.Notification.Exceptions
{
    public class NotificationOptionsException : Exception
    {
        private readonly ValidationResult _validationResult;

        public NotificationOptionsException(string message, ValidationResult validationResult) : base(message)
        {
            _validationResult = validationResult;
        }

        public ValidationFailure[] Errors => _validationResult?.Errors.ToArray();

        public static NotificationOptionsException FromValidationResults(ValidationResult validationResult)
        {
            if (validationResult == null) throw new ArgumentNullException(nameof(validationResult));

            return new NotificationOptionsException("Error in NotificationOptions. Check the required fields in appsettings.json", validationResult);
        }
    }
}
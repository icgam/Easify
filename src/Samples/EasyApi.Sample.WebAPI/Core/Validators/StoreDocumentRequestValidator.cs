using System.Collections.Generic;
using EasyApi.Extensions;
using EasyApi.Sample.WebAPI.Domain;
using FluentValidation;

namespace EasyApi.Sample.WebAPI.Core.Validators
{
    public sealed class StoreDocumentRequestValidator : AbstractValidator<StoreDocumentsRequest>
    {
        private readonly HashSet<string> _supportedOperations = new HashSet<string>(new List<string>
        {
            "Validate",
            "Process",
            "Publish"
        });

        public StoreDocumentRequestValidator()
        {
            RuleFor(m => m.RequestId).NotEmpty();
            RuleFor(m => m.Operation).Must(OneOfSupportedOperations).WithMessage(GetValidOperationsMessage);
            RuleFor(c => c.Owner).NotNull().SetValidator(new OwnerValidator());
            RuleFor(c => c.Documents).NotNull().NotEmpty().SetCollectionValidator(new DocumentValidator());
        }

        private string GetValidOperationsMessage(StoreDocumentsRequest request)
        {
            if (request.Operation.AnyValue())
            {
                return
                    $"'{request.Operation}' is not supported. Supported operations are: {string.Join(", ", _supportedOperations)}.";
            }

            return $"Operation must be supplied! Supported operations are: {string.Join(", ", _supportedOperations)}.";
        }

        private bool OneOfSupportedOperations(string operation)
        {
            return _supportedOperations.Contains(operation);
        }
    }
}
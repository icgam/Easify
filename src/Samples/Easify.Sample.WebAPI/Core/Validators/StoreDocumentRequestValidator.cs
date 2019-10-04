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
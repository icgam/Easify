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
using System.Linq;
using FluentValidation.Results;

namespace Easify.Configurations
{
    public class InvalidConfigurationException : Exception
    {
        public InvalidConfigurationException(string message, ValidationResult validationResult) 
            : base(BuildValidationMessage(message, validationResult))
        {
        }        
        
        public InvalidConfigurationException(ValidationResult validationResult) 
            : base(BuildValidationMessage("Validation Error", validationResult))
        {
        }

        private static string BuildValidationMessage(string message, ValidationResult validationResult)
        {
            var messages =
                validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToArray();

            var validationMessage = string.Join($",{System.Environment.NewLine}", messages);
            return $"{message} - {validationMessage}";
        }
    }
}
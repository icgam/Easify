using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EasyApi.AspNetCore.ActionFilters
{
    public sealed class UnprocessableEntityObjectResult : ObjectResult
    {
        private const int UnprocessableEntity = 422;

        public UnprocessableEntityObjectResult(ModelStateDictionary modelState)
            : base(new SerializableError(modelState))
        {
            if (modelState == null)
                throw new ArgumentNullException(nameof(modelState));

            StatusCode = UnprocessableEntity;
        }
    }
}
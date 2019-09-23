using System;

namespace EasyApi.AspNetCore.RequestCorrelation.Core
{
    public sealed class GuidBasedCorrelationIdProvider : ICorrelationIdProvider
    {
        public string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
using System;

namespace EasyApi.AspNetCore.RequestCorrelation.Core.OptionsBuilder
{
    public static class CorrelationOptionsBuilderExtensions
    {
        public static IExcludeRequests ExcludeDefaultUrls(this IExcludeRequests excludeRequests)
        {
            if (excludeRequests == null) throw new ArgumentNullException(nameof(excludeRequests));

            excludeRequests
                .ExcludeUrl(f => f.WhenEndsWith("/health"))
                .ExcludeUrl(f => f.WhenEndsWith("/diagnostics/logs"))
                .ExcludeUrl(f => f.WhenContains("/swagger"));

            return excludeRequests;
        }
    }
}
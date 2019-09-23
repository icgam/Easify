using System;
using System.Collections.Generic;
using EasyApi.AspNetCore.RequestCorrelation.Core.OptionsBuilder;

namespace EasyApi.AspNetCore.RequestCorrelation.Domain
{
    public sealed class RequestCorrelationOptions
    {
        public RequestCorrelationOptions(bool forceCorrelation, IEnumerable<ICheckUrl> urlsToIgnoreRules)
        {
            ForceCorrelation = forceCorrelation;
            UrlsToIgnoreRules = urlsToIgnoreRules ?? throw new ArgumentNullException(nameof(urlsToIgnoreRules));
        }

        public bool ForceCorrelation { get; }
        public IEnumerable<ICheckUrl> UrlsToIgnoreRules { get; }
    }
}
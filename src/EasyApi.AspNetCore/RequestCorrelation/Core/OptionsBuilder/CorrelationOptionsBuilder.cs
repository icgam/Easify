using System;
using System.Collections.Generic;
using EasyApi.AspNetCore.RequestCorrelation.Domain;

namespace EasyApi.AspNetCore.RequestCorrelation.Core.OptionsBuilder
{
    public sealed class CorrelationOptionsBuilder : IBuildOptions, ICorrelateRequests, IExcludeRequests
    {
        private readonly List<ICheckUrl> _urlRules = new List<ICheckUrl>();
        private bool _forceCorrelation = true;

        public RequestCorrelationOptions Build()
        {
            return new RequestCorrelationOptions(_forceCorrelation, _urlRules);
        }

        public IBuildOptions EnforceCorrelation()
        {
            _forceCorrelation = true;
            return this;
        }

        public IBuildOptions AutoCorrelateRequests()
        {
            _forceCorrelation = false;
            return this;
        }

        public IExcludeRequests ExcludeUrl(Func<ISetUrlFilter, ICheckUrl> filterProvider)
        {
            if (filterProvider == null) throw new ArgumentNullException(nameof(filterProvider));
            var filter = filterProvider(new UrlRule());
            _urlRules.Add(filter);
            return this;
        }
    }
}
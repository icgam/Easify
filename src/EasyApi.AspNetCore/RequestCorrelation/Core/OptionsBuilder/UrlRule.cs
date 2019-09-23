using System;

namespace EasyApi.AspNetCore.RequestCorrelation.Core.OptionsBuilder
{
    public sealed class UrlRule : ICheckUrl, ISetUrlFilter
    {
        private Func<string, bool> _filter = url => true;

        public bool UrlMatches(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            var uri = new Uri(url);
            return _filter(uri.AbsolutePath);
        }

        public ICheckUrl WhenStartsWith(string urlFragment)
        {
            if (string.IsNullOrWhiteSpace(urlFragment))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(urlFragment));
            
            _filter = url => url.StartsWith(urlFragment, StringComparison.CurrentCultureIgnoreCase);
            return this;
        }

        public ICheckUrl WhenEndsWith(string urlFragment)
        {
            if (string.IsNullOrWhiteSpace(urlFragment))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(urlFragment));

            _filter = url => url.EndsWith(urlFragment, StringComparison.CurrentCultureIgnoreCase);
            return this;
        }

        public ICheckUrl WhenContains(string urlFragment)
        {
            if (string.IsNullOrWhiteSpace(urlFragment))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(urlFragment));

            _filter = url => url.IndexOf(urlFragment, StringComparison.CurrentCultureIgnoreCase) >= 0;
            return this;
        }
    }
}
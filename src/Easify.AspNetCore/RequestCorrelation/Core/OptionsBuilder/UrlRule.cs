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
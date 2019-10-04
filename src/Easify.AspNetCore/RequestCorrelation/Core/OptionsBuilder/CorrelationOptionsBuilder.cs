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
using System.Collections.Generic;
using Easify.AspNetCore.RequestCorrelation.Domain;

namespace Easify.AspNetCore.RequestCorrelation.Core.OptionsBuilder
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
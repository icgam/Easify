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

using System.Net.Http.Headers;
using Microsoft.Net.Http.Headers;
using RestEase;
using HttpHeaders = Easify.Http.HttpHeaders;

namespace Easify.RestEase.Client
{
    // TODO: Is this worth to have a separate simple class
    // TODO: Extend this with the with extra properties for security & ...
    public interface IRestClient
    {
        [Header(HttpHeaders.HttpRequestId)] 
        string CorrelationId { get; set; }
        
        [Header(HeaderNames.Authorization)]
        AuthenticationHeaderValue Authorization { get; set; }
    }
}
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

using System.Collections.Generic;
using System.Threading.Tasks;
using Easify.RestEase.Client;
using RestEase;

namespace Easify.Sample.WebAPI.Core
{
    public interface IValuesClient : IRestClient
    {
        [Get("api/Values")]
        Task<IEnumerable<string>> GetValuesAsync();

        [Get("api/Values/{id}")]
        Task<string> GetValueAsync([Path] int id);

        [Post("api/Values/{id}")]
        Task PostValueAsync([Path] int id, [Body] string value);

        [Put("api/Values/{id}")]
        Task PutValueAsync([Path] int id, [Body] string value);

        [Delete("api/Values/{id}")]
        Task DeleteValueAsync([Path] int id);
    }
}
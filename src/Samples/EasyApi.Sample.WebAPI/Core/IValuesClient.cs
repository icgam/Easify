// This software is part of the EasyApi framework
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

﻿using System.Collections.Generic;
using System.Threading.Tasks;
using EasyApi.RestEase.Client;
using RestEase;

namespace EasyApi.Sample.WebAPI.Core
{
    public interface IValuesClient : IRestClient
    {
        [Get("api/values")]
        Task<IEnumerable<string>> GetValues();

        [Get("api/values/{id}")]
        Task<string> GetValue([Path] int id);

        [Get("api/values/{id}")]
        Task PostValue([Path] int id, [Body] string value);

        [Get("api/values/{id}")]
        Task PutValue([Path] int id, [Body] string value);

        [Get("api/values/{id}")]
        Task DeleteValue([Path] int id);
    }
}

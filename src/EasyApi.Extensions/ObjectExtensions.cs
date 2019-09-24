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

﻿using System;
using System.Reflection;

namespace EasyApi.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsValueType(this object source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var type = source.GetType();
            return type.GetTypeInfo().IsValueType && type.GetTypeInfo().IsPrimitive;
        }

        public static bool IsReferenceType(this object source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.GetType().GetTypeInfo().IsClass && source is string == false;
        }

        public static bool IsString(this object source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.GetType().GetTypeInfo().IsClass && source is string;
        }
    }
}
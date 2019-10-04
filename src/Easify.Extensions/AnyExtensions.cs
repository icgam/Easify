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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyApi.Extensions
{
    public static class AnyExtensions
    {
        public static bool Empty<T>(this T data)
        {
            return AnyValue(data) == false;
        }

        public static bool AnyValue<T>(this T data)
        {
            if (data == null)
            {
                return false;
            }

            var stringData = data as string;
            if (stringData != null && string.IsNullOrWhiteSpace(stringData))
            {
                return false;
            }

            if (IsEnumerable(data))
            {
                var enumerable = data as IEnumerable;
                if (enumerable == null || enumerable.IsEnumerableEmpty())
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsEnumerable<T>(T data)
        {
            var type = data.GetType();
            return type.IsEnumerable();
        }

        public static bool IsEnumerable(this Type type)
        {
            return (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ||
                   type.GetTypeInfo().ImplementedInterfaces
                       .Any(ti => ti.GetTypeInfo().IsGenericType
                                  && ti.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        public static bool IsEnumerableEmpty(this IEnumerable items)
        {
            var enumerator = items.GetEnumerator();
            return !enumerator.MoveNext();
        }
    }
}
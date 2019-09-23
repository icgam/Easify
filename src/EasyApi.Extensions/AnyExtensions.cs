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
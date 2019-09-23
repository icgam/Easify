using System;
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
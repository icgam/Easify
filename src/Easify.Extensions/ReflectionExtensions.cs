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
using System.Linq;
using System.Reflection;

namespace EasyApi.Extensions
{
    public static class ReflectionExtensions
    {
        public static string GetAssemblyPrefix(this Type type, string nameSectionSeparator)
        {
            var fullname = type.GetTypeInfo().Assembly.FullName;
            return fullname.Substring(0, fullname.IndexOf(nameSectionSeparator, StringComparison.Ordinal) + 1);
        }

        public static IEnumerable<Assembly> GetReferencedAssemblies(this Type type, string assemblyNameStartsWith)
        {
            return type.GetTypeInfo().Assembly.GetReferencedAssemblies(assemblyNameStartsWith);
        }

        public static IEnumerable<Assembly> GetReferencedAssembliesFromType<T>(this T source,
            string assemblyNameStartsWith)
            where T : class
        {
            var type = typeof(T);
            return type.GetReferencedAssemblies(assemblyNameStartsWith);
        }

        public static IEnumerable<Assembly> GetReferencedAssemblies(this Assembly assembly,
            string assemblyNameStartsWith)
        {
            var assemblies = new List<Assembly> {assembly};

            var references =
                assembly.GetReferencedAssemblies()
                    .Where(a => a.Name.StartsWith(assemblyNameStartsWith, StringComparison.CurrentCultureIgnoreCase))
                    .ToList();

            foreach (var assemblyName in references)
            {
                var childAssembly = Assembly.Load(assemblyName);
                var children = GetReferencedAssemblies(childAssembly, assemblyNameStartsWith);

                assemblies.AddRange(children);
            }

            return assemblies;
        }
    }
}
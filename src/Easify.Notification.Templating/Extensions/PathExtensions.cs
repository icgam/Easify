using System;
using System.IO;
using System.Reflection;

namespace Easify.Notification.Templating.Extensions
{
    public static class PathExtensions
    {
        public static string GetRelativePathToType<T>(this string filename, bool checkExistance = false)
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));

            var location = Path.GetDirectoryName(typeof(T).GetTypeInfo().Assembly.Location);
            var target = Path.Combine(location, filename);

            if (checkExistance && !File.Exists(target))
                throw new FileNotFoundException($"File not found in {target}");

            return target;
        }

        public static string GetRelativePath(this string filename, bool checkExistance = false)
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));

            var location = AppDomain.CurrentDomain.BaseDirectory;
            var target = Path.Combine(location, filename);

            if (checkExistance && !File.Exists(target))
                throw new FileNotFoundException($"File not found in {target}");

            return target;
        }
    }
}
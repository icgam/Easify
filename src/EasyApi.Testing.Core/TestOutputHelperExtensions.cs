using Newtonsoft.Json;
using Xunit.Abstractions;

namespace EasyApi.Testing.Core
{
    public static class TestOutputHelperExtensions
    {
        public static void Write<T>(this ITestOutputHelper output, T objectToWrite)
        {
            output.WriteLine(JsonConvert.SerializeObject(objectToWrite, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }
    }
}

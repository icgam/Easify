using System;
using System.Collections.Generic;
using System.IO;

namespace EasyApi.Sample.WebAPI.IntegrationTests.Helpers
{
    public sealed class DynamicLoggingFixture : IDisposable
    {
        public string LogDirectoryPath { get; private set; }
        public string LogFilePath { get; private set; }

        public TestServerFixture<TStartup> CreateServer<TStartup>() where TStartup : class
        {
           var fixture = TestServerFixture<TStartup>.CreateWithLoggingEnabled();
            LogFilePath = fixture.LogFilePath;
            LogDirectoryPath = fixture.LogDirectoryPath;
            return fixture;
        }

        public IEnumerable<string> GetLogFileContents()
        {
            try
            {
                return File.ReadLines(LogFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                Directory.Delete(LogDirectoryPath, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

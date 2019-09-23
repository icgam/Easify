using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EasyApi.Sample.WebAPI.IntegrationTests.Helpers;
using EasyApi.Testing.Core;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace EasyApi.Sample.WebAPI.IntegrationTests
{
    public sealed class DynamicLogLevelsViaLogControllerTests
    {
        public DynamicLogLevelsViaLogControllerTests(ITestOutputHelper output)
        {
            Output = output;
        }

        private ITestOutputHelper Output { get; }

        private async Task<HttpResponseMessage> ChangeLogLevel(string levelToSet,
            TestServerFixture<StartupForIntegration> serverFixture)
        {
            var responseForLevelChange =
                await serverFixture.Client.PostAsync("diagnostics/logs", new JsonContent(new
                {
                    LoggingLevel = levelToSet
                }));
            return responseForLevelChange;
        }

        /// <summary>
        ///     This is fairly complex test due to the asynchrony issues when using GLOBAL logger instance, so it is difficult to
        ///     separate tests
        ///     and consistantly execute them since parallel test executions wipes out the logger instance.
        /// </summary>
        /// <returns></returns>
        [Trait("Category", "NO_CI")]
        [Fact]
        public async Task GivenLogger_WhenLogLevelsDynamicallySwitched_ThenLoggerShouldFilterLogMessages()
        {
            // Arrange         
            using (var fixture = new DynamicLoggingFixture())
            {
                var logLevels = new List<string>();

                using (var serverFixture = fixture.CreateServer<StartupForIntegration>())
                {
                    // Act
                    var responseForLevelChange = await ChangeLogLevel("Warning", serverFixture);
                    var response = await serverFixture.Client.GetAsync("api/logs");

                    Assert.Equal(HttpStatusCode.OK, responseForLevelChange.StatusCode);
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                    logLevels.Add(Log.Logger.GetActiveLogLevel());

                    responseForLevelChange = await ChangeLogLevel("Fatal", serverFixture);
                    response = await serverFixture.Client.GetAsync("api/logs");

                    Assert.Equal(HttpStatusCode.OK, responseForLevelChange.StatusCode);
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                    logLevels.Add(Log.Logger.GetActiveLogLevel());

                    responseForLevelChange = await ChangeLogLevel("Verbose", serverFixture);
                    response = await serverFixture.Client.GetAsync("api/logs");

                    Assert.Equal(HttpStatusCode.OK, responseForLevelChange.StatusCode);
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                    logLevels.Add(Log.Logger.GetActiveLogLevel());


                    // Assert
                    Assert.Equal(HttpStatusCode.OK, responseForLevelChange.StatusCode);
                }

                var logs = fixture.GetLogFileContents().SkipWhile(m => !m.EndsWith("[Warning] Warning")).ToList();

                Output.Write(new
                {
                    fixture.LogFilePath,
                    LogLevelsUsed = logLevels,
                    Logs = logs
                });

                Assert.Collection(logs, l => { Assert.EndsWith("[Warning] Warning", l); },
                    l => { Assert.EndsWith("[Error] Error", l); }, l => { Assert.EndsWith("[Fatal] Critical", l); },
                    l => { Assert.EndsWith("[Fatal] Critical", l); }, l => { }, l => { }, l => { }, l => { },
                    l => { Assert.EndsWith("[Verbose] Trace", l); }, l => { Assert.EndsWith("[Debug] Debug", l); },
                    l => { Assert.EndsWith("[Information] Information", l); },
                    l => { Assert.EndsWith("[Warning] Warning", l); }, l => { Assert.EndsWith("[Error] Error", l); },
                    l => { Assert.EndsWith("[Fatal] Critical", l); }, l => { });
            }
        }
    }
}
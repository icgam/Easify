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

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Easify.Sample.WebAPI.IntegrationTests.Helpers;
using Easify.Testing.Extensions;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace Easify.Sample.WebAPI.IntegrationTests
{
    public sealed class DynamicLogLevelsViaLogControllerTests
    {
        public DynamicLogLevelsViaLogControllerTests(ITestOutputHelper output)
        {
            Output = output;
        }

        private ITestOutputHelper Output { get; }

        private async Task<HttpResponseMessage> ChangeLogLevel(string levelToSet,
            TestApplicationFactory<StartupForIntegration> fixture)
        {
            var responseForLevelChange =
                await fixture.CreateClient().PostAsync("diagnostics/logs", new JsonContent(new
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
        //[Trait("Category", "NO_CI")]
        //[Fact]
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
                    var response = await serverFixture.CreateClient().GetAsync("api/logs");

                    Assert.Equal(HttpStatusCode.OK, responseForLevelChange.StatusCode);
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                    logLevels.Add(Log.Logger.GetActiveLogLevel());

                    responseForLevelChange = await ChangeLogLevel("Fatal", serverFixture);
                    response = await serverFixture.CreateClient().GetAsync("api/logs");

                    Assert.Equal(HttpStatusCode.OK, responseForLevelChange.StatusCode);
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                    logLevels.Add(Log.Logger.GetActiveLogLevel());

                    responseForLevelChange = await ChangeLogLevel("Verbose", serverFixture);
                    response = await serverFixture.CreateClient().GetAsync("api/logs");

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
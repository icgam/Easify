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
using System.Threading.Tasks;
using Easify.AspNetCore.UnitTests.Helpers;
using Easify.Testing.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Easify.AspNetCore.UnitTests.Logging.SeriLog
{
    public sealed class LogInterceptorTests : IClassFixture<ServiceCollectionFixture>
    {
        public LogInterceptorTests(ITestOutputHelper output, ServiceCollectionFixture fixture)
        {
            Output = output;
            Fixture = fixture;
        }

        private ITestOutputHelper Output { get; }
        private ServiceCollectionFixture Fixture { get; }

        [Fact]
        public async Task GivenSut_WhenAsyncMethodCalledAndThrows_ThenLogFailedExecution()
        {
            // Arrange
            var provider = Fixture.BuildProvider(logLevel => logLevel >= LogLevel.Debug);
            var sut = provider.GetRequiredService<IFakeService>();

            // Act
            await Assert.ThrowsAsync<FakeException>(() => sut.CallAndThrowAsync());

            // Assert
            Output.Write(Fixture.Log.Logs);

            Assert.Collection(Fixture.Log.Logs, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains("Executing 'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndThrowAsync' ...", s);
            }, s =>
            {
                Assert.Contains("Error", s);
                Assert.Contains(
                    "Failed to execute 'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndThrowAsync'!" +
                    " 'Easify.AspNetCore.UnitTests.Helpers.FakeException' has been thrown.", s);
            });

            Assert.Equal("Changed", sut.State);
        }

        [Fact]
        public async Task
            GivenSut_WhenAsyncMethodCalledWithoutParameters_AndLogLevelSetToTrace_ThenLogThatMethodHasNoParameters()
        {
            // Arrange
            var provider = Fixture.BuildProvider(logLevel => logLevel >= LogLevel.Trace);
            var sut = provider.GetRequiredService<IFakeService>();

            // Act
            await sut.CallAsync();

            // Assert
            Output.Write(Fixture.Log.Logs);

            Assert.Collection(Fixture.Log.Logs, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains("Executing 'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAsync' ...", s);
            }, s =>
            {
                Assert.Contains("Trace", s);
                Assert.Contains("Method called with NO PARAMETERS.", s);
            }, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains("'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAsync' execution completed.", s);
            }, s =>
            {
                Assert.Contains("Trace", s);
                Assert.Contains(
                    "'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAsync' is VOID and has no return value.", s);
            });

            Assert.Equal("Changed", sut.State);
        }

        [Fact]
        public async Task
            GivenSut_WhenAsyncMethodWith_CustomClass_ArgumentsCalled_ThenLogMethodArguments_AndReturnValue()
        {
            // Arrange
            var argument = new FakeArgument
            {
                Value = Guid.NewGuid().ToString()
            };

            var provider = Fixture.BuildProvider(logLevel => logLevel >= LogLevel.Trace);
            var sut = provider.GetRequiredService<IFakeService>();

            // Act
            await sut.CallAndReturnResultAsync(argument);

            // Assert
            Output.Write(Fixture.Log.Logs);

            Assert.Collection(Fixture.Log.Logs, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains(
                    "Executing 'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndReturnResultAsync' ...", s);
            }, s =>
            {
                Assert.Contains("Trace", s);
                Assert.Contains(argument.Value, s);
            }, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains(
                    "'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndReturnResultAsync' execution completed.",
                    s);
            }, s =>
            {
                Assert.Contains("Trace", s);
                Assert.Contains(argument.Value, s);
            });

            Assert.Equal("Changed", sut.State);
        }

        [Fact]
        public async Task GivenSut_WhenAsyncMethodWith_Integer_ArgumentsCalled_ThenLogMethodArguments_AndReturnValue()
        {
            // Arrange
            const int value = 3;
            var provider = Fixture.BuildProvider(logLevel => logLevel >= LogLevel.Trace);
            var sut = provider.GetRequiredService<IFakeService>();

            // Act
            await sut.CallAndReturnResultAsync(value);

            // Assert
            Output.Write(Fixture.Log.Logs);

            Assert.Collection(Fixture.Log.Logs, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains(
                    "Executing 'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndReturnResultAsync' ...", s);
            }, s =>
            {
                Assert.Contains("Trace", s);
                Assert.Contains($"Execution parameters: {value}.", s);
            }, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains(
                    "'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndReturnResultAsync' execution completed.",
                    s);
            }, s =>
            {
                Assert.Contains("Trace", s);
                Assert.Contains(
                    $"Result for 'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndReturnResultAsync' is: {value}.",
                    s);
            });

            Assert.Equal("Changed", sut.State);
        }

        [Fact]
        public async Task GivenSut_WhenAsyncMethodWith_String_ArgumentsCalled_ThenLogMethodArguments_AndReturnValue()
        {
            // Arrange
            const string value = "3";
            var provider = Fixture.BuildProvider(logLevel => logLevel >= LogLevel.Trace);
            var sut = provider.GetRequiredService<IFakeService>();

            // Act
            await sut.CallAndReturnResultAsync(value);

            // Assert
            Output.Write(Fixture.Log.Logs);

            Assert.Collection(Fixture.Log.Logs, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains(
                    "Executing 'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndReturnResultAsync' ...", s);
            }, s =>
            {
                Assert.Contains("Trace", s);
                Assert.Contains($"Execution parameters: {value}.", s);
            }, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains(
                    "'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndReturnResultAsync' execution completed.",
                    s);
            }, s =>
            {
                Assert.Contains("Trace", s);
                Assert.Contains(
                    $"Result for 'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndReturnResultAsync' is: {value}.",
                    s);
            });

            Assert.Equal("Changed", sut.State);
        }

        [Fact]
        public async Task GivenSut_WhenAsyncMethodWithArgumentsCalledAndThrows_ThenLogFailedExecution()
        {
            // Arrange
            const string value = "value";
            var provider = Fixture.BuildProvider(logLevel => logLevel >= LogLevel.Trace);
            var sut = provider.GetRequiredService<IFakeService>();

            // Act
            await Assert.ThrowsAsync<FakeException>(() => sut.CallAndThrowInsteadOfReturnResultAsync(value));

            // Assert
            Output.Write(Fixture.Log.Logs);

            Assert.Collection(Fixture.Log.Logs, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains(
                    "Executing 'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndThrowInsteadOfReturnResultAsync' ...",
                    s);
            }, s =>
            {
                Assert.Contains("Trace", s);
                Assert.Contains($"Execution parameters: {value}.", s);
            }, s =>
            {
                Assert.Contains("Error", s);
                Assert.Contains(
                    "Failed to execute 'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndThrowInsteadOfReturnResultAsync'!" +
                    " 'Easify.AspNetCore.UnitTests.Helpers.FakeException' has been thrown.", s);
            });

            Assert.Equal("Changed", sut.State);
        }

        [Fact]
        public void GivenSut_WhenSyncMethodCalled_ThenLogMethodExecutionStartAndEnd()
        {
            // Arrange
            var provider = Fixture.BuildProvider(logLevel => logLevel >= LogLevel.Debug);
            var sut = provider.GetRequiredService<IFakeService>();


            // Act
            sut.Call();

            // Assert
            Output.Write(Fixture.Log.Logs);

            Assert.Collection(Fixture.Log.Logs, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains("Executing 'Easify.AspNetCore.UnitTests.Helpers.FakeService.Call' ...", s);
            }, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains("'Easify.AspNetCore.UnitTests.Helpers.FakeService.Call' execution completed.", s);
            });

            Assert.Equal("Changed", sut.State);
        }

        [Fact]
        public void GivenSut_WhenSyncMethodCalledAndLogLevelSetToDebug_ThenLogThatMethodHasNoParametersAndReturnValue()
        {
            // Arrange
            var provider = Fixture.BuildProvider(logLevel => logLevel >= LogLevel.Debug);
            var sut = provider.GetRequiredService<IFakeService>();

            // Act
            sut.Call();

            // Assert
            Output.Write(Fixture.Log.Logs);

            Assert.Collection(Fixture.Log.Logs, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains("Executing 'Easify.AspNetCore.UnitTests.Helpers.FakeService.Call' ...", s);
            }, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains("'Easify.AspNetCore.UnitTests.Helpers.FakeService.Call' execution completed.", s);
            });

            Assert.Equal("Changed", sut.State);
        }

        [Fact]
        public void GivenSut_WhenSyncMethodCalledAndThrows_ThenLogFailedExecution()
        {
            // Arrange
            var provider = Fixture.BuildProvider(logLevel => logLevel >= LogLevel.Debug);
            var sut = provider.GetRequiredService<IFakeService>();

            // Act
            Assert.Throws<FakeException>(() => sut.CallAndThrow());

            // Assert
            Output.Write(Fixture.Log.Logs);

            Assert.Collection(Fixture.Log.Logs, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains("Executing 'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndThrow' ...", s);
            }, s =>
            {
                Assert.Contains("Error", s);
                Assert.Contains("Failed to execute 'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndThrow'!" +
                                " 'Easify.AspNetCore.UnitTests.Helpers.FakeException' has been thrown.", s);
            });

            Assert.Equal("Changed", sut.State);
        }

        [Fact]
        public void
            GivenSut_WhenSyncMethodCalledWithoutParameters_AndLogLevelSetToTrace_ThenLogThatMethodHasNoParameters()
        {
            // Arrange
            var provider = Fixture.BuildProvider(logLevel => logLevel >= LogLevel.Trace);
            var sut = provider.GetRequiredService<IFakeService>();

            // Act
            sut.Call();

            // Assert
            Output.Write(Fixture.Log.Logs);

            Assert.Collection(Fixture.Log.Logs, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains("Executing 'Easify.AspNetCore.UnitTests.Helpers.FakeService.Call' ...", s);
            }, s =>
            {
                Assert.Contains("Trace", s);
                Assert.Contains("Method called with NO PARAMETERS.", s);
            }, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains("'Easify.AspNetCore.UnitTests.Helpers.FakeService.Call' execution completed.", s);
            }, s =>
            {
                Assert.Contains("Trace", s);
                Assert.Contains(
                    "'Easify.AspNetCore.UnitTests.Helpers.FakeService.Call' is VOID and has no return value.", s);
            });

            Assert.Equal("Changed", sut.State);
        }

        [Fact]
        public void GivenSut_WhenSyncMethodWith_CustomClass_ArgumentsCalled_ThenLogMethodArguments_AndReturnValue()
        {
            // Arrange
            var argument = new FakeArgument
            {
                Value = Guid.NewGuid().ToString()
            };

            var provider = Fixture.BuildProvider(logLevel => logLevel >= LogLevel.Trace);
            var sut = provider.GetRequiredService<IFakeService>();

            // Act
            sut.CallAndReturnResult(argument);

            // Assert
            Output.Write(Fixture.Log.Logs);

            Assert.Collection(Fixture.Log.Logs, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains("Executing 'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndReturnResult' ...",
                    s);
            }, s =>
            {
                Assert.Contains("Trace", s);
                Assert.Contains(argument.Value, s);
            }, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains(
                    "'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndReturnResult' execution completed.", s);
            }, s =>
            {
                Assert.Contains("Trace", s);
                Assert.Contains(argument.Value, s);
            });

            Assert.Equal("Changed", sut.State);
        }

        [Fact]
        public void GivenSut_WhenSyncMethodWith_Integer_ArgumentsCalled_ThenLogMethodArguments_AndReturnValue()
        {
            // Arrange
            const int value = 3;
            var provider = Fixture.BuildProvider(logLevel => logLevel >= LogLevel.Trace);
            var sut = provider.GetRequiredService<IFakeService>();

            // Act
            sut.CallAndReturnResult(value);

            // Assert
            Output.Write(Fixture.Log.Logs);

            Assert.Collection(Fixture.Log.Logs, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains("Executing 'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndReturnResult' ...",
                    s);
            }, s =>
            {
                Assert.Contains("Trace", s);
                Assert.Contains($"Execution parameters: {value}.", s);
            }, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains(
                    "'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndReturnResult' execution completed.", s);
            }, s =>
            {
                Assert.Contains("Trace", s);
                Assert.Contains(
                    $"Result for 'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndReturnResult' is: {value}.",
                    s);
            });

            Assert.Equal("Changed", sut.State);
        }


        [Fact]
        public void GivenSut_WhenSyncMethodWith_String_ArgumentsCalled_ThenLogMethodArguments_AndReturnValue()
        {
            // Arrange
            const string value = "3";
            var provider = Fixture.BuildProvider(logLevel => logLevel >= LogLevel.Trace);
            var sut = provider.GetRequiredService<IFakeService>();

            // Act
            sut.CallAndReturnResult(value);

            // Assert
            Output.Write(Fixture.Log.Logs);

            Assert.Collection(Fixture.Log.Logs, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains("Executing 'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndReturnResult' ...",
                    s);
            }, s =>
            {
                Assert.Contains("Trace", s);
                Assert.Contains($"Execution parameters: {value}.", s);
            }, s =>
            {
                Assert.Contains("Debug", s);
                Assert.Contains(
                    "'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndReturnResult' execution completed.", s);
            }, s =>
            {
                Assert.Contains("Trace", s);
                Assert.Contains(
                    $"Result for 'Easify.AspNetCore.UnitTests.Helpers.FakeService.CallAndReturnResult' is: {value}.",
                    s);
            });

            Assert.Equal("Changed", sut.State);
        }
    }
}
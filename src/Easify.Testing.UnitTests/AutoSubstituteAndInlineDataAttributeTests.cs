﻿using AutoFixture.Xunit2;
using Easify.Testing.UnitTests.Helpers;
using NSubstitute;
using Xunit;

namespace Easify.Testing.UnitTests
{
    public class AutoSubstituteAndInlineDataAttributeTests
    {
        [Theory]
        [AutoSubstituteAndInlineData]
        public void ShouldGenerateAutoData(string data)
        {
            // Arrange
            // Act
            // Assert
            Assert.False(string.IsNullOrEmpty(data));
        }

        [Theory]
        [AutoSubstituteAndInlineData("value1")]
        public void ShouldGenerateAutoDataAndManualData(string value, string data)
        {
            // Arrange
            // Act
            // Assert
            Assert.False(string.IsNullOrEmpty(data));
            Assert.Equal("value1", value);
        }

        [Theory]
        [AutoSubstituteAndInlineData("value1")]
        public void ShouldGenerateAutoDataAndManualDataAndDependency(string value, string data, IMyService myDependency)
        {
            // Arrange
            // Act
            // Assert
            Assert.False(string.IsNullOrEmpty(data));
            Assert.IsAssignableFrom<IMyService>(myDependency);
            Assert.Equal("value1", value);
        }

        [Theory]
        [AutoSubstituteAndInlineData("value1")]
        public void ShouldGenerateAutoDataAndManualDataAndSutWithDependency(string value, string data,
            [Frozen] IMyService myDependency,
            MyRootService sut)
        {
            // Arrange
            // Act
            sut.DoWork(value);
            // Assert
            Assert.Equal("value1", value);
            myDependency.Received(1).DoWork(Arg.Is<string>(p => p == value));
            Assert.NotEmpty(data);
        }

        [Theory]
        [AutoSubstituteAndInlineData("value1")]
        [AutoSubstituteAndInlineData("value2")]
        [AutoSubstituteAndInlineData("value3")]
        public void ShouldGenerateAutoDataAndManualDataAndSutWithDependencyForAllValues(string value, string data,
            [Frozen] IMyService myDependency,
            MyRootService sut)
        {
            // Arrange
            // Act
            sut.DoWork(value);
            // Assert
            myDependency.Received(1).DoWork(Arg.Is<string>(p => p == value));
            Assert.NotEmpty(data);
        }

        [Theory]
        [InlineAutoData("value1", "value1")]
        [InlineAutoData("value2", "value2")]
        [InlineAutoData("value3", "value3")]
        public void ShouldGenerateAutoDataAndManualDataForAllValues(string expectedValue, string passedInValue,
            string data)
        {
            // Arrange
            // Act
            // Assert
            Assert.Equal(expectedValue, passedInValue);
            Assert.NotEmpty(data);
        }

        [Theory]
        [InlineData("value1", "value1")]
        [InlineData("value2", "value2")]
        [InlineData("value3", "value3")]
        public void ShouldGenerateManualDataForAllValues(string expectedValue, string passedInValue)
        {
            // Arrange
            // Act
            // Assert
            Assert.Equal(expectedValue, passedInValue);
        }
    }
}
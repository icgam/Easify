using AutoFixture.Xunit2;
using Easify.Testing.UnitTests.Helpers;
using NSubstitute;
using Xunit;

namespace Easify.Testing.UnitTests
{
    public class AutoSubstituteAndDataAttributeTests
    {
        [Theory]
        [AutoSubstituteAndData]
        public void ShouldGenerateAutoData(string data)
        {
            // Arrange
            // Act
            // Assert
            Assert.False(string.IsNullOrEmpty(data));
        }

        [Theory]
        [AutoSubstituteAndData]
        public void ShouldGenerateAutoDataAndDependency(string data, IMyService myDependency)
        {
            // Arrange
            // Act
            // Assert
            Assert.False(string.IsNullOrEmpty(data));
            Assert.IsAssignableFrom<IMyService>(myDependency);
        }

        [Theory]
        [AutoSubstituteAndData]
        public void ShouldGenerateAutoDataAndSutWithDependency(string data, [Frozen] IMyService myDependency, MyRootService sut)
        {
            // Arrange
            // Act
            sut.DoWork();
            // Assert
            myDependency.Received(1).DoWork();
            Assert.NotEmpty(data);
        }
    }
}
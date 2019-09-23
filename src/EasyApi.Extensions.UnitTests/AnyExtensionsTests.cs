using System.Collections.Generic;
using Xunit;

namespace EasyApi.Extensions.UnitTests
{
    public class AnyExtensionsTests
    {
        [Fact]
        public void ShouldReturnTrueForInitializedObject()
        {
            // Arrange
            var data = new object();
            // Act
            // Assert
            Assert.True(data.AnyValue());
        }

        [Fact]
        public void ShouldReturnTrueForCollectionWithItems()
        {
            // Arrange
            var data = new List<string>()
            {
                "value"
            };
            // Act
            // Assert
            Assert.True(data.AnyValue());
        }

        [Fact]
        public void ShouldReturnFalseForNullObject()
        {
            // Arrange
            object data = null;
            // Act
            // Assert
            Assert.False(data.AnyValue());
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldReturnFalseForString(string data)
        {
            // Arrange
            // Act
            // Assert
            Assert.False(data.AnyValue());
        }

        [Fact]
        public void ShouldReturnFalseForEmptyCollection()
        {
            // Arrange
            var data = new List<string>();
            // Act
            // Assert
            Assert.False(data.AnyValue());
        }

        [Fact]
        public void ShouldReturnTrueForNull()
        {
            // Arrange
            object data = null;
            // Act
            // Assert
            Assert.True(data.Empty());
        }

        [Fact]
        public void ShouldReturnTrueForEmptyString()
        {
            // Arrange
            string data = string.Empty;
            // Act
            // Assert
            Assert.True(data.Empty());
        }

        [Fact]
        public void ShouldReturnTrueForEmptyCollection()
        {
            // Arrange
            var data = new List<object>();
            // Act
            // Assert
            Assert.True(data.Empty());
        }

        [Fact]
        public void ShouldReturnTrueForIEnumerableInterface()
        {
            // Arrange
            var type = typeof(IEnumerable<>);
            // Act
            // Assert
            Assert.True(type.IsEnumerable());
        }

        [Fact]
        public void ShouldReturnTrueForEmptyEnumerable()
        {
            // Arrange
            var source = new List<string>();
            // Act
            // Assert
            Assert.True(source.IsEnumerableEmpty());
        }

        [Fact]
        public void ShouldReturnFalseForEmptyEnumerable()
        {
            // Arrange
            var source = new List<string>()
            {
                "data"
            };
            // Act
            // Assert
            Assert.False(source.IsEnumerableEmpty());
        }

        [Fact]
        public void ShouldReturnFalseForAnyValueIfNullableTypeHasNoValue()
        {
            // Arrange
            var source = new decimal?();
            // Act
            // Assert
            Assert.False(source.AnyValue());
        }

        [Fact]
        public void ShouldReturnTrueForAnyValueIfNullableTypeHasValue()
        {
            // Arrange
            var source = new decimal?(0);
            // Act
            // Assert
            Assert.True(source.AnyValue());
        }
    }
}
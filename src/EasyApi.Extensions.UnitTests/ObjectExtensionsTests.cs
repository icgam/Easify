using Xunit;

namespace EasyApi.Extensions.UnitTests
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void IntegerShouldReturnFalseForIsReferenceType()
        {
            // Assemble
            const int value = 1;
            // Act
            var result = value.IsReferenceType();
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IntegerShouldReturnFalseForIsString()
        {
            // Assemble
            const int value = 1;
            // Act
            var result = value.IsString();
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IntegerShouldReturnTrueForIsValueType()
        {
            // Assemble
            const int value = 1;
            // Act
            var result = value.IsValueType();
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ObjectShouldReturnFalseForIsValueType()
        {
            // Assemble
            var value = new object();
            // Act
            var result = value.IsValueType();
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ObjectShouldReturnFalseForIsString()
        {
            // Assemble
            var value = new object();
            // Act
            var result = value.IsString();
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ObjectShouldReturnTrueForIsReferenceType()
        {
            // Assemble
            var value = new object();
            // Act
            var result = value.IsReferenceType();
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void StringShouldReturnFalseForIsReferenceType()
        {
            // Assemble
            var value = "string1";
            // Act
            var result = value.IsReferenceType();
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void StringShouldReturnFalseForIsValueType()
        {
            // Assemble
            var value = "string1";
            // Act
            var result = value.IsValueType();
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void StringShouldReturnTrueForIsString()
        {
            // Assemble
            var value = "string1";
            // Act
            var result = value.IsString();
            // Assert
            Assert.True(result);
        }
    }
}
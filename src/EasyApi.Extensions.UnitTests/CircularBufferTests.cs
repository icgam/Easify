using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EasyApi.Extensions.UnitTests
{
    public sealed class CircularBufferTests
    {
        [Fact]
        public void GivenEmptyBufferWhenItemAddedThenShoulReturnItem()
        {
            // Arrange
            var sut = new CircularBuffer<int>(5) {1};

            // Act

            // Assert
            Assert.Collection(sut, v => { Assert.Equal(1, v); });
        }

        [Fact]
        public void GivenItemAddedWhenBufferOverflownThenDiscardOldestAndRetainNewlyAddedValue()
        {
            // Arrange
            var sut = new CircularBuffer<int>(5) {1, 2, 3, 4, 5, 6};
            // Act

            // Assert
            Assert.Collection(sut, v => { Assert.Equal(2, v); }, v => { Assert.Equal(3, v); },
                v => { Assert.Equal(4, v); }, v => { Assert.Equal(5, v); }, v => { Assert.Equal(6, v); });
        }

        [Fact(Skip = "Due to locking issues test is temporarily disabled")]
        public async Task GivenMultipleThreadsWhenAddingValuesThenSynchronizeThreads()
        {
            // Arrange
            const int bufferSize = 10000;
            var sut = new CircularBuffer<string>(bufferSize);

            // Act
            var tasks = new List<Task>();

            for (var taskId = 0; taskId < 5000; taskId++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var messageIndex = 0; messageIndex < 1000; messageIndex++)
                    {
                        sut.Add($"message.{messageIndex}");
                    }
                }));
            }

            await Task.WhenAll(tasks);

            // Assert
            Assert.Equal(bufferSize, sut.Count());
        }
    }
}
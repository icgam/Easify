// This software is part of the EasyApi framework
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

﻿using System.Collections.Generic;
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
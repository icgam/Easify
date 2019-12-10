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
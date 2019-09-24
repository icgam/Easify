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

﻿using Xunit;

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
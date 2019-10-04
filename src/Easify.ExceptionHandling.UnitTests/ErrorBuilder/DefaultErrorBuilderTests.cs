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
using System.Collections.Generic;
using EasyApi.ExceptionHandling.Domain;
using EasyApi.ExceptionHandling.ErrorBuilder;
using Xunit;

namespace EasyApi.ExceptionHandling.UnitTests.ErrorBuilder
{
    public sealed class DefaultErrorBuilderTests
    {
        [Fact]
        public void GivenAnyExceptionWhenExceptionIsNotSuppliedThenBuilderShouldThrow()
        {
            // Arrange
            var sut = new DefaultErrorBuilder<Exception>();
            
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => sut.Build(null, new List<Error>(), false));
        }

        [Fact]
        public void GivenAnyExceptionWhenInternalErrorsAreNotSuppliedThenBuilderShouldThrow()
        {
            // Arrange
            var sut = new DefaultErrorBuilder<Exception>();
            
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => sut.Build(new Exception(), null, false));
        }

        [Fact]
        public void GivenAnyExceptionWhenErrorIsBuiltThenErrorWithExceptionContentsShouldBeCreated()
        {
            // Arrange
            var sut = new DefaultErrorBuilder<Exception>();
            var exception = new Exception("My Error");
            var internalErrors = new List<Error>();

            // Act
            var result = sut.Build(exception, internalErrors, false);

            // Assert
            Assert.Equal("My Error", result.Message);
            Assert.Equal("Exception", result.ErrorType);
            Assert.Same(internalErrors, result.ChildErrors);
        }
    }
}

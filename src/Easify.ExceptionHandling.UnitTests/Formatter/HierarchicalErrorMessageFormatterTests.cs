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
using EasyApi.ExceptionHandling.Formatter;
using NSubstitute;
using Xunit;

namespace Easify.ExceptionHandling.UnitTests.Formatter
{
    public sealed class HierarchicalErrorMessageFormatterTests
    {
        private IErrorMessageFormatterOptions GetOptions(string symbol = " ")
        {
            var options = Substitute.For<IErrorMessageFormatterOptions>();
            options.IndentBy.Returns(symbol);
            return options;
        }

        [Fact]
        public void ShouldExtractAndCorrectlyFormatAggregateErrorMessages()
        {
            // Arrange
            var internalError1 = new Error("Internal app error 1", "Exception");
            var internalError2 = new Error("Internal app error 2", "Exception");

            var exception = new Error("Our App Error", "Exception", new List<Error>
            {
                internalError1,
                internalError2
            });

            var expectedMessage = string.Join(
                Environment.NewLine,
                "Our App Error",
                " Internal app error 1",
                " Internal app error 2");

            var options = GetOptions();
            var sut = new HierarchicalErrorMessageFormatter(options);

            // Act
            var result = sut.FormatErrorMessages(exception);

            // Assert
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public void ShouldExtractAndCorrectlyFormatDeepErrorHierarchies()
        {
            // Arrange
            var leafError1 = new Error("(Leaf) Internal app error 1", "Exception");
            var leafError2 = new Error("(Leaf) Internal app error 2", "Exception");
            var leafError4 = new Error("(Leaf) Internal app error 4", "Exception");

            var branchError1 = new Error("(Branch) app error 1", "Exception", leafError1);
            var branchError2 = new Error("(Branch) app error 2", "Exception", leafError2);
            var rootError = new Error("(Root) app error", "Exception", new List<Error>
            {
                branchError1,
                branchError2,
                leafError4
            });

            var expectedMessage = string.Join(
                Environment.NewLine,
                "(Root) app error",
                " (Branch) app error 1",
                "  (Leaf) Internal app error 1",
                " (Branch) app error 2",
                "  (Leaf) Internal app error 2",
                " (Leaf) Internal app error 4");

            var options = GetOptions();
            var sut = new HierarchicalErrorMessageFormatter(options);

            // Act
            var result = sut.FormatErrorMessages(rootError);

            // Assert
            Assert.Equal(expectedMessage, result);
        }


        [Fact]
        public void ShouldExtractAndCorrectlyFormatMultilineErrorMessages()
        {
            // Arrange
            var internalError1 = new Error(string.Join(
                Environment.NewLine,
                "Internal Error",
                "Internal Error Details"), "Exception");
            var exception = new Error(string.Join(
                Environment.NewLine,
                "Our App Error",
                "Our App Error Details"), "Exception", internalError1);

            var expectedMessage = string.Join(
                Environment.NewLine,
                "Our App Error",
                "Our App Error Details",
                " Internal Error",
                " Internal Error Details");

            var options = GetOptions();
            var sut = new HierarchicalErrorMessageFormatter(options);

            // Act
            var result = sut.FormatErrorMessages(exception);

            // Assert
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public void ShouldReturnOriginalAndNestedErrorMessageWithCorrectIndentation()
        {
            // Arrange
            var options = GetOptions();
            var internalError = new Error("Internal app error", "Exception");
            var error = new Error("Our App Error", "Exception", internalError);

            var expectedMessage = string.Join(
                Environment.NewLine,
                "Our App Error",
                " Internal app error");
            var sut = new HierarchicalErrorMessageFormatter(options);

            // Act
            var result = sut.FormatErrorMessages(error);

            // Assert
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public void ShouldReturnOriginalErrorMessage()
        {
            // Arrange
            var error = new Error("Our App Error", "Exception");
            var options = GetOptions();
            var sut = new HierarchicalErrorMessageFormatter(options);

            // Act
            var result = sut.FormatErrorMessages(error);

            // Assert
            Assert.Equal("Our App Error", result);
        }

        [Fact]
        public void ShouldExtractAndFormatErrorMessagesUsingConfiguredIndentationSymbol()
        {
            // Arrange
            var internalError1 = new Error("Internal app error 1", "Exception");
            var exception = new Error("Our App Error", "Exception", new List<Error>
            {
                internalError1
            });

            var expectedMessage = string.Join(
                Environment.NewLine,
                "Our App Error",
                "///Internal app error 1");

            var options = GetOptions("///");
            var sut = new HierarchicalErrorMessageFormatter(options);

            // Act
            var result = sut.FormatErrorMessages(exception);

            // Assert
            Assert.Equal(expectedMessage, result);
        }
    }
}
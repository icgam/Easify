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

using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Easify.Notification.Templating.UnitTests
{
    public class FileBasedTemplateProviderTests
    {
        [Fact] 
        public async Task GivenTemplate_WhenCallingGetTemplateContent_ShouldReturnCorrectContent()
        {
            // Arrange
            var templatePath = "template.hb";
            var expected = "Hello {{Title}} test at {{date AsOf}}";

            var sut = new FileBasedTemplateProvider();

            // Act
            var actual = await sut.GetTemplateContentAsync(templatePath);

            // Assert
            actual.Should().Be(expected);
        }
    }
}
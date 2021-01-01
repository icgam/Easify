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
using System.Threading.Tasks;
using Easify.Notification.Templating.HandleBars;
using FluentAssertions;
using Xunit;

namespace Easify.Notification.Templating.UnitTests
{
    public class HandleBarsTemplateContentRendererTests
    {
        [Theory]
        [InlineData("Hello {{Title}} test at {{date AsOf}}", "Hello RenderAsync test at 21/09/2018")]
        public async Task Should_RenderAsync_ReturnTheRightRenderedOutput(string template, string expected)
        {
            // Given
            var model = new {AsOf = new DateTime(2018, 9, 21, 4, 30, 30), Title = "RenderAsync" };
            var definition = new TemplateDefinition("sampleTemplate", template);

            var sut = new HandleBarsTemplateContentRenderer();

            // When
            var actual = await sut.RenderAsync(definition, model);

            // Then
            actual.Should().NotBeNull();
            actual.Content.Should().Be(expected);
            actual.Data.Should().Be(model);
        }
    }
}
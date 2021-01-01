using System;
using System.Threading.Tasks;
using FluentAssertions;
using ICG.Core.Templating.HandleBars;
using Xunit;

namespace ICG.Core.Templating.UnitTests
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
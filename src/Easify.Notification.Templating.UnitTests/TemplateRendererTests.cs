using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Easify.Notification.Templating.UnitTests
{
    public class TemplateRendererTests
    {
        [Theory]
        [InlineData("Hello {{Title}} test at {{date AsOf}}", "Hello RenderAsync test at 21/09/2018")]
        public async Task Should_RenderTemplateAsync_ReturnTheRightTemplate(string template, string expected)
        {
            // Given
            var model = new Model();

            var templateProvider = Substitute.For<ITemplateProvider>();
            templateProvider.GetTemplateContentAsync(Arg.Any<string>()).Returns(Task.FromResult(template));

            var templateContentRenderer = Substitute.For<ITemplateContentRenderer>();
            templateContentRenderer.RenderAsync(Arg.Any<TemplateDefinition>(), model).Returns(Task.FromResult(new RenderedTemplate<Model>(expected, model)));

            var sut = new TemplateRenderer(templateProvider, templateContentRenderer);

            // When
            var actual = await sut.RenderTemplateAsync("TemplateName", "TemplatePath", model);

            // Then
            actual.Should().NotBeNull();
            actual.Content.Should().Be(expected);
            actual.Data.Should().Be(model);
        }

        private class Model
        {
            public DateTime AsOf { get; set; } = new DateTime(2018, 9, 21, 4, 30, 30);
            public string Title { get; set; } = "RenderAsync";
        }
    }
}
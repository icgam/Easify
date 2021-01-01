using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Easify.Notification.Templating.UnitTests
{
    public class FileBasedTemplateProviderTests
    {
        [Fact]
        public async Task Should__ReturnTheRightTemplate()
        {
            // Given
            var templatePath = "template.hb";
            var expected = "Hello {{Title}} test at {{date AsOf}}";

            var sut = new FileBasedTemplateProvider();

            // When
            var actual = await sut.GetTemplateContentAsync(templatePath);

            // Then
            actual.Should().Be(expected);
        }
    }
}
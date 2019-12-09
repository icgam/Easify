using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace Easify.Testing
{
    public class AutoSubstituteAndDataAttribute : AutoDataAttribute
    {
        public AutoSubstituteAndDataAttribute() : base(() => new Fixture().Customize(new AutoNSubstituteCustomization()))
        {
        }
    }
}

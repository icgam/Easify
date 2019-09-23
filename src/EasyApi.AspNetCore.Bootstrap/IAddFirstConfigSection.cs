
namespace EasyApi.AspNetCore.Bootstrap
{
    public interface IAddFirstConfigSection
    {
        IAddExtraConfigSection AddConfigSection<TSection>()
            where TSection : class, new();

        IAddExtraConfigSection AddConfigSection<TSection>(string section)
            where TSection : class, new();
    }
}
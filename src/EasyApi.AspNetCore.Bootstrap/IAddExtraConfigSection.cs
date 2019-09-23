namespace EasyApi.AspNetCore.Bootstrap
{
    public interface IAddExtraConfigSection : IConfigureApplicationBaseException
    {
        IAddExtraConfigSection AndSection<TSection>()
            where TSection : class, new();

        IAddExtraConfigSection AndSection<TSection>(string section)
            where TSection : class, new();
    }
}
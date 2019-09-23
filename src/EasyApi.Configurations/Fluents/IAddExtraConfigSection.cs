namespace EasyApi.Configurations.Fluents
{
    public interface IAddExtraConfigSection : IBuildConfiguration
    {
        IAddExtraConfigSection And<TSection>()
            where TSection : class, new();

        IAddExtraConfigSection And<TSection>(string section)
            where TSection : class, new();
    }
}
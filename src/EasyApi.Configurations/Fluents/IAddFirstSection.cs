namespace EasyApi.Configurations.Fluents
{
    public interface IAddFirstSection : IBuildConfiguration
    {
        IAddExtraConfigSection AddSection<TSection>()
            where TSection : class, new();

        IAddExtraConfigSection AddSection<TSection>(string section)
            where TSection : class, new();
    }
}
namespace EasyApi.Bootstrap
{
    // TODO: Needs to be changed
    public interface IConfigureContainer
    {
        IBootstrapApplication UseContainer<TContainer>(
            ContainerFactory<TContainer> containerFactory) where TContainer : class;
    }
}
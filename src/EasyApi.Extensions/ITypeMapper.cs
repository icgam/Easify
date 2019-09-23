namespace EasyApi.Extensions
{
    // TODO: Should have a base implementation of this as we do in many different applications
    public interface ITypeMapper
    {
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination); // TODO: They are redundant with no usage.
        TDestination Map<TSource, TDestination>(TSource source); // TODO: They are redundant with no usage.
        TDestination Map<TDestination>(object source);
    }
}

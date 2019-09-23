## Object Mapping ([Back to Index](https://github.com/icgam/ICG.DotNetCore.Framework))

One of the most BORING and error prone tasks is Object Mapping! Which is why [Automapper](http://automapper.org/) is preconfigured within the framework without any extra effort from your side. In order to use [Automapper](http://automapper.org/) you need to define the mappings you need as shown below:

```csharp
public sealed class Startup
{
    // Code omitted for clarity

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        return services.BootstrapApp<Startup>(Configuration,
            app => app
                .HandleApplicationException<MyApplicationBaseException>()
                .ConfigureMappings(c =>
                {
                    c.CreateMap<PersonEntity, PersonDO>();
                })
                .AddServices((container, config) => {})
        );
    }

    // Code omitted for clarity
}
```

In the example above we have a mapping for PersonEntity to PersonDO object configured. Obviousle this is a very simple mapping and you will most likely have many more mappings defined, or perhaps much more complex mappings, in which case we advice you to use [Automapper](http://automapper.org/) [*PROFILES*](https://github.com/AutoMapper/AutoMapper/wiki/Configuration) for encapsulating the configuration. Framework will integrate [Automapper](http://automapper.org/) with the IOC container your application is using, which enables you to inject any service that is available to your IOC container to be injected into your mapping code. Lets examine example below:

```csharp
public sealed class Startup
{
    // Code omitted for clarity

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        return services.BootstrapApp<Startup>(Configuration,
            app => app
                .HandleApplicationException<MyApplicationBaseException>()
                .ConfigureMappings(c =>
                {
                    c.CreateMap<AssetEntity, AssetDO>().ConvertUsing<AssetConverter>();
                })
                .AddServices((container, config) => {
                    container.AddTransient<IRateProvider, DummyRateProvider>();
                    container.AddTransient<ITypeConverter<AssetEntity, AssetDO>, AssetConverter>();
                })
        );
    }

    // Code omitted for clarity
}
```

And the *AssetConverter* implementation:

```csharp
public sealed class AssetConverter : ITypeConverter<AssetEntity, AssetDO> {
    private readonly IRateProvider _rateProvider;
    public AssetConverter(IRateProvider rateProvider)
    {
        if (rateProvider == null) throw new ArgumentNullException(nameof(rateProvider));
        _rateProvider = rateProvider;
    }
    public AssetDO Convert(AssetEntity source, AssetDO destination, ResolutionContext context)
    {
        if (source == null)
        {
            return null;
        }
        return new AssetDO
        {
            Id = source.Id,
            Rating = _rateProvider.GetRating(source.Id)
        };
    }
}
```

As we can observe *IRateProvider* service in to our *AssetConverter* service. This is taken care for you by IOC/[Automapper](http://automapper.org/) integration. There are a few things you still need to do however. At the moment of writing this documentation you need to manually register your *TypeConverter / ValueResolver / etc* in the **AddServices** section, as show in the code sample above. Obviously you also need to register any service that you want to inject, in this case *IRateProvider* service.

In the future versions we will attempt to preregister all [Automapper](http://automapper.org/) *TypeConverters and ValueResolvers*.

As we know usage of Global Static's is highly discouraged and is considered to be an *anti-pattern*! Another thing we want to avoid is taking dependecies on 3rd party frameworks such as [Automapper](http://automapper.org/) in our projects. This is why we have defined **ITypeMapper** interface, that is preregistered with our IOC container and can be injected anywhere we need to perform mapping operations within our application code. Look at the example below:

```csharp
public class MyAssetService
{
    private readonly ITypeMapper _mapper;

    public MyAssetService(ITypeMapper mapper)
    {
        if (mapper == null) throw new ArgumentNullException(nameof(mapper));
        _mapper = mapper;
    }

    public AssetDO GetAsset(string assetId)
    {
        var assetToMap = new AssetEntity { Id = assetId };
        return _mapper.Map<AssetDO>(assetToMap);
    }
}
```

[Back to Index](https://github.com/icgam/ICG.DotNetCore.Framework)
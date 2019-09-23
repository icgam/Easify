## Configuration API ([Back to Index](https://github.com/icgam/ICG.DotNetCore.Framework))

Goal for the configuration API is to load GENERAL config settings, as well as environment specific config settings, which can be achieved by adding following code:

```csharp
public class Startup {

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    // Code omitted for clarity
}
```

This will preconfigure the Asp.NET Core Options API which enables us to load custom sections from our appsettings.json and/or appsettings.{ENV}.json config files. Lets say we have the following config:

```json
{
  "Section1": {
    "Value1": "1",
    "Value2": "2"
  }
}
```

To be able to read following values we need 3 things:

* To have a model class that matches the config structure
    
```csharp
    public class Section1
    {
        public string Value1 { get; set; }
        public string Value2 { get; set; }
    }
```

* To inform our framework of the intention of loading the config section using this model by invoking **AddConfigSection<>** on our app builder component:

```csharp
    public class Startup
    {
        // Code omitted for clarity

        public IConfigurationRoot Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BootstrapApp<Startup>(Configuration,
                app => app
                    .AddConfigSection<Section1>()
                    .HandleApplicationException<BaseAppException>()
                    .AddServices((container, config) => {})
            );
        }

        // Code omitted for clarity
    }
```

* In order to use this configuration section we simply need to inject **IOptions<Section1>** to any service that is registered with our IOC container like so:

```csharp
    public sealed class MyService
    {
        private readonly IOptions<Section1> _section;

        public MyService(IOptions<Section1> section)
        {
            _section = section;
        }
    }
```

To add additional sections we need to extend our configuration using **AndSection<>()** method on app builder API:

```csharp
public sealed class Startup
{
    // Code omitted for clarity

    public IConfigurationRoot Configuration { get; }
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        return services.BootstrapApp<Startup>(Configuration,
            app => app
                .AddConfigSection<Section1>()
                .AndSection<Section2>()
                .AndSection<Section3>()
                .HandleApplicationException<BaseAppException>()
                .AddServices((container, config) => {})
        );
    }

    // Code omitted for clarity
}
```

You may wonder how does the framework know which config section to load!? Currently it simply looks for a **CASE INSENSITIVE** match for a class name in the config, so **Section1** class name is matched to section with the same name in the **\*.json** or **\*.{ENV}.json** files:

```json
{
  "Section1": {
    "Value1": "1",
    "Value2": "2"
  }
}
```

In order to load NESTED sections all you need to do is to provide a full path the section of interested. Given we have following configuration:

```json
{
  "Section1": {
      "SubSectionA": {
        "Value1": "1",
        "Value2": "2"
      },
      "SubSectionB": {
        "Value1": "3",
        "Value2": "4"
      }
  }
}
```

If we want to load *SubSectionB* we would achieve this using following configuration:

```csharp
    public class Startup
    {
        // Code omitted for clarity

        public IConfigurationRoot Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BootstrapApp<Startup>(Configuration,
                app => app
                    .AddConfigSection<Section1>("Section1:SubSectionA")
                    .HandleApplicationException<BaseAppException>()
                    .AddServices((container, config) => {})
            );
        }

        // Code omitted for clarity
    }
```

> Note config section name passed as a parameter to the *AddConfigSection* method. Each level is separated with a COLON.

### Default Configurations
By default some configurations are registered in the container which is injectable to dependent classes:

```json
{
  "Application": {
    "Title": "Application Name",
    "Version": "Application version. Like 1, 2 or 1.4",
    "Url": "Url of the application if its an API othewise leave this empty",
    "Environment": {
      "Name": "Environment: Development, Integration, Uat, Staging and Production", 
      "DnsPrefix": "prefix of the DNS such a .int.domain.com. Leave empty if its not an API"
    }
  }
}
```

This will be injectable to any class which give the basic information regarding to the application itself:

```csharp
    public class SampleService
    {
        public SampleService(IOptions<Application> optionAccessor) 
        {
            // Use the application data for different purposes
        }
    }
```


[Back to Index](https://github.com/icgam/ICG.DotNetCore.Framework)
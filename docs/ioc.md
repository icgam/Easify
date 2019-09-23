## IOC ([Back to Index](https://github.com/icgam/ICG.DotNetCore.Framework))

IOC is at heart of [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) and we leverage it in every aspect of our framework. Currently we have two choices for IOC containers:

* Going NATIVE


### Native Container

If NATIVE [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) container is choosen, we dont need to do anything extra to enabled it. All of the examples above uses NATIVE container.

#### Interception

As of recent changes in the framework, we have interception using native container. In order to access this feature you will need to install **ICG.Core.Foil** package, at which point you will have several extensions for **IServiceCollection** that will allow you to register interceptors as shown in the below example:

```csharp
public sealed class Startup
{
    // Code omitted for clarity

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        return services.BootstrapApp<Startup>(Configuration,
            app => app
                .HandleApplicationException<MyApplicationBaseException>()
                .AddServices((container, config) =>
                {
                    container.AddTransient<IMyService, MyService>(
                        by => by.InterceptBy<LogInterceptor>());
                })
        );
    }

    // Code omitted for clarity
}
```

The **LogInterceptor** shown in the example can be found in **ICG.AspNetCore.Logging.SeriLog** package. If you wish to implement your own interceptor you will need to derive from **AsyncInterceptorBase** class and implement the required methods.

We use interception heavily to instrument components, which enables us to get insights into what is happening inside our application for no extra price for developer. This is especially useful when we need to troubleshoot an application or explain certain behahviours.

### Support for other IoC
To support other IOC container they can be used as extensions on ServiceCollection to import registration into new containers. Below are some samples:
* Autofac: https://github.com/autofac/Autofac.Extensions.DependencyInjection
* Stashbox: https://github.com/z4kn4fein/stashbox-extensions-dependencyinjection

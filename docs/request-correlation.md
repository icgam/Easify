## Request Correlation

Framework is built to support MicroService architecture. One of the challenges we face in such architectures is tracking the flow of the requests through multiple API's. This particular feature of the API is built to address **Request Correlation** problem. Default behaviour of the API is to expect that each INCOMING request has a correlation id, which is located in the request header. We support two header types: **X-Correlation-ID** and **X-Request-ID**. There is no difference which one you are using, however it is MANDATORY to have one, otherwise requests will be rejected with an error explanaining that Correlation ID was not present.

If you want to override this behaviour, then instead of rejecting a NON correlated request we can generate an id ourselves and attached it to the request. In order to achieve this we should configure our API as shown below:

```csharp
public sealed class Startup
{
    // Code omitted for clarity

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        return services.BootstrapApp<Startup>(Configuration,
            app => app
                .HandleApplicationException<MyApplicationBaseException>()
                .ConfigureCorrelation(o => o.AutoCorrelateRequests())
                .AddServices((container, config) => {})
        );
    }

    // Code omitted for clarity
}
```

We also have some additional options here, for example we may want to exclude certain URL's (i.e. not correlate them), example being a PING url or some diagnostics endpoint, to achieve this please consult an example below:

```csharp
public sealed class Startup
{
    // Code omitted for clarity

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        return services.BootstrapApp<Startup>(Configuration,
            app => app
                .HandleApplicationException<MyApplicationBaseException>()
                .ConfigureCorrelation(o => o.ExcludeUrl(f => f.WhenContains("/diagnostics/ping")))
                .AddServices((container, config) => {})
        );
    }

    // Code omitted for clarity
}
```

With this configuration all requests to URL's that contains *'/diagnostics/ping'* in their path will not be checked for correlation ID's. We have 3 different filtering options:

* WhenContains
* WhenStartsWith
* WhenEndsWith

I believe those are self explanatory. You may chain as many url fragments as you like.

[Back to Index](https://github.com/icgam/ICG.DotNetCore.Framework)
## Exception Handling ([Back to Index](https://github.com/icgam/ICG.DotNetCore.Framework))

Ideally our applications wouldn't need the functionality :) However experience shows that such APP's rarely exists if at all. First let's start with the goals for our exception handling strategy:

* Show full details in DEV environment
* Surface only user friendly exceptions in non DEV environments
* Replace any non user friendly exceptions with pre-configured error message in non DEV environments
* Allow to choose the level of detail we want to expose via error messages
* Flow exception through number of API's to the caller

Basically we want to have API's that can distinguish between *SYSTEM ERRORS* and *USER ERRORS*, first one being simply a fault which user has no control over and is not able to do anything about it. Best bet is to contant IT support. *USER ERRORS* on the other hand should tell exactly what went wrong and how to fix it. In order to achieve this we need to tell our application what is a friendly exception, in other words when an exception is thrown, how to know if it should be surfaced or not? To achieve this we need help from the developer of the API's, so we ask the developer to create a **MyApplicationBaseException** which derives from **Exception** class and tell us about this exception type when the API is configured:

```csharp
public sealed class Startup
{
    // Code omitted for clarity

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        return services.BootstrapApp<Startup>(Configuration,
            app => app
                .HandleApplicationException<MyApplicationBaseException>()
                .AddServices((container, config) => {})
        );
    }

    // Code omitted for clarity
}
```

You are DONE! This will treat any exception that is derived from **MyApplicationBaseException** class as a user friendly error and will return it with **4xx** status code and the contents of the exception message and its nested exception messages. There is a little bit more that is going on behind the scenes here. When message hierarchy is built all exceptions in the hierarchy that are not *user friendly* will simply be skipped. Let say we encountered following error:

* MyApplicationException1
  * AggregateException
    * Exception
    * MyApplicationException2

The resulting message will look like so:

* MyApplicationException1
  * MyApplicationException2

The actual error structure that is returned to the user is:

```csharp
public sealed class Startup
{
    public sealed class ErrorResponse
    {
        public string Message { get; set; }
        public IEnumerable<Error> UserErrors { get; set; }
        public IEnumerable<Error> RawErrors { get; set; }
    }
}
```

* The message property will have a nicely formatted errors ready to be displayed in a UI. (Always returned)
* UserErrors will contain hierarchy of *user friendly* error messages, if you wish to format them to your own liking (Only returned if the LevelOfDetail is set to **StandardMessage**)
* RawErrors normally will be empty, however incase it's enabled, it will contain FULL exception messages hierarchy (Returned when LevelOfDetail is set to **DetailedErrors**)

What if you we have more than one exception type we want to treat as user friendly? This may sound odd at first, I mean we have a base exception for our entire application and thats it right? Well, not necessarily, we may have a 3rd party plugin or other API that throws exceptions we want to surface to our user, but have no control over base types. In that case we can handle additional exceptions by simply chaining *.AndHandle<>()* method after our app base exception has been configured like so:

```csharp
public sealed class Startup
{
    // Code omitted for clarity

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        return services.BootstrapApp<Startup>(Configuration,
            app => app
                .HandleApplicationException<MyApplicationBaseException>()
                .AndHandle<ThirdPartyPluginException>()
                .AddServices((container, config) => {})
        );
    }

    // Code omitted for clarity
}
```

We may handle one or more additional exceptions, currently there is no limit on it, however normally you wouldn't expect to have more than 2 or 3 exception types configured here.

We may also have more advanced case, say we want to treat exception as *user friendly* only if certain conditions are met. A good example of that is **ApiException**, we only want to forward this if it has a StatusCode **4xx**, but not if its **5xx**, due to the fact that users can't do anything about server errors. We can filter exceptions by supplying a predicate:

```csharp
public sealed class Startup
{
    // Code omitted for clarity

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        return services.BootstrapApp<Startup>(Configuration,
            app => app
                .HandleApplicationException<MyApplicationBaseException>()
                .AndHandle<ApiException>(f => f.StatusCode == HttpStatusCode.BadRequest)
                .AddServices((container, config) => {})
        );
    }

    // Code omitted for clarity
}
```

The last and most complex case, which I will touch only ligthly are custom error builders. We may wanna handle an exception and override the default error builder, which essensially allows us to craft a custom error message based on the information we have in the exception. A good example of this is handling **ApiException**, since our desired LevelOfError may or may not be the same as the child API that throw the error, we need to recalculate error hierarchy for **ApiException** per API. This is handled for you by the framework already, but if you encounter cases where you do need such behaviour, you will need to implement **IErrorBuilder<TException>** and provide an instance of it via an overload available in **AndHandle<IErrorBuilder>(b => b.Use(*new MyErrorBuilder()*))**.

[Back to Index](https://github.com/icgam/ICG.DotNetCore.Framework)
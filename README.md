# EasyApi

EasyApi is a set of libraries which facilitate different aspects of a Restful/Microservice api and takes away the boilerplate configuration/bootstrap code that needs to be written each time a new API project is setup. There are up's and down's to this approach, however the benfit of this approach is to setup consistent api projects faster and easier.

Boilerplate api provides the following features:

* Global Error Handling
* Preconfigured Logging to a rolling file and log agregartors such as loggly and seq
* Application/Common services preregistration within IOC container
* Request Correlation
* Preconfigured MVC pipeline
* FluentValidation framework integration
* AutoMapper integration with desired IOC container
* RestEase REST client integration
* Swagger integration
* Health endpoint exposed
* Diagnostics endpoint

For using the full capability of the project install *EasyApi.AspNetCore.Bootstrap* nuget package.

```cmd

dotnet add package EasyApi.AspNetCore.Bootstrap

or
 
Install-Package EasyApi.AspNetCore.Bootstrap

```

In order to achieve all of this functionality you merely need a few lines of code. At this point your *Program.cs* should look like this:

```csharp


    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.Run<Startup>(s =>  s.ConfigureLogger<Startup>());
        }
    }

```

 And *startup.cs* as follows:

 ```csharp

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BootstrapApp<Startup>(Configuration,
                            app => app
                                .HandleApplicationException<TemplateApiApplicationException>()
                                .AddServices((container, config) => { })
                        );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDefaultApiPipeline(Configuration, env, loggerFactory);
        }
    }

 ```

As you may see there are quite a few features enabled with very few lines of code. Let us take a deeper look at the template and let see what else it offers. Here is a list of topics we will be discussing in detail, so please feel free to skip to any section you maybe interested in:

* [Configuration API](docs/configuration.md)
* [Logging](docs/logging.md)
* [Exception Handling](docs/exception-handling.md)
* [Object Mapping](docs/object-mapping.md)
* [Validation](docs/validation.md)
* [Request Correlation](docs/request-correlation.md)
* [REST Client](docs/rest-client.md)
* [Cors](docs/cors.md)
* [IOC](docs/ioc.md)
* [API Documentation](docs/api-documentation.md)

Also there are couple of useful interfaces, tools and utilities which you can use which they are already available in the framework.
* [Core Services](docs/core-services.md)

Thank you for reading and let's build some API's ...

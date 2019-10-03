# EasyApi

EasyApi is a set of libraries which facilitate different aspects of a Restful/Microservice api and takes away the boilerplate configuration/bootstrap code that needs to be written each time a new API project is setup. There are up's and down's to this approach, however the benefit of this approach is to setup consistent api projects faster and easier.

Boilerplate api provides the following features:

* Global Error Handling
* Preconfigured Logging to a rolling file and log aggregartors such as loggly and seq
* Application/Common services preregistration within IOC container
* Request Correlation
* Preconfigured MVC pipeline
* FluentValidation framework integration
* AutoMapper integration with desired IOC container
* RestEase REST client integration
* Swagger integration
* Health endpoint exposed
* Diagnostics endpoint

For using the full benefit of the library, Create a simple asp.net core project and install *EasyApi.AspNetCore.Bootstrap* nuget package.

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

The project/solution is ready to be running in visual studio or using dotnet cli.

More detail information can be found in [wiki](wiki)


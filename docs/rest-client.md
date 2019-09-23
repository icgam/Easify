## REST Client ([Back to Index](https://github.com/icgam/ICG.DotNetCore.Framework))

The main idea behind *MicroServices* architecture is unsurprisingly to have many small highly cohesive services or API's, meaning we will be performing many REST calls :) Writing REST clients is a tedious and time consuming task, which is why most of the time developers use a well known EDITOR INHERITANCE technique otherwise known as COPY-PASTE. Our framework addresses this issue by removing the need to write all this code. In order to achieve this we use an excellent [RestEase](https://github.com/canton7/RestEase) framework. I guess it's best to look at a code sample to understand how [RestEase](https://github.com/canton7/RestEase) works. Let's take a controller we want to write a client for:

```csharp
[Route("api/[controller]")]
public class ValuesController : Controller
{
    // GET api/values
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }
    // GET api/values/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }
    // POST api/values
    [HttpPost]
    public void Post([FromBody]string value)
    {
    }
    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody]string value)
    {
    }
    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
```

Client for such controller would look like so:

```csharp
public interface IValuesClient
{
    [Get("api/values")]
    Task<IEnumerable<string>> GetValues();

    [Get("api/values/{id}")]
    Task<string> GetValue([Path] int id);

    [Get("api/values/{id}")]
    Task PostValue([Path] int id, [Body] string value);

    [Get("api/values/{id}")]
    Task PutValue([Path] int id, [Body] string value);

    [Get("api/values/{id}")]
    Task DeleteValue([Path] int id);
}
```

Great uh? Well, but what about our correlation fo requests? How do we flow through the Correlation-ID? Don't worry, this is super easy! :) All we need to do, is to implement **IRestClient** and the rest is taken care for us. So our finalized client looks like so:

```csharp
public interface IValuesClient : IRestClient
{
    [Get("api/values")]
    Task<IEnumerable<string>> GetValues();

    [Get("api/values/{id}")]
    Task<string> GetValue([Path] int id);

    [Get("api/values/{id}")]
    Task PostValue([Path] int id, [Body] string value);

    [Get("api/values/{id}")]
    Task PutValue([Path] int id, [Body] string value);

    [Get("api/values/{id}")]
    Task DeleteValue([Path] int id);
}
```

In order to use this client, we obviously need to inject it to whichever service wants to use it. However before we can inject it, we also need to register it with our IOC container. Now our client needs to know our external service *BaseUrl*, which can be supplied in one of two ways:

* By supplying it via lambda:

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
                        container.AddRestClient<IValuesClient>(() => "http://company.com/api");
                    })
            );
        }

        // Code omitted for clarity
    }
```

* Or by providing a config section where it is specified:

```csharp
    public sealed class Startup
    {
        // Code omitted for clarity

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BootstrapApp<Startup>(Configuration,
                app => app
                    .AddConfigSection<Clients>()
                    .HandleApplicationException<MyApplicationBaseException>()
                    .AddServices((container, config) =>
                    {
                        container.AddRestClient<IValuesClient, Clients>(c => c.ProducerClientUrl);
                    })
            );
        }

        // Code omitted for clarity
    }
```
    Note that the config section must be registered in order to be used by the RestClient.
    Clients is a simple POCO that you define to hold client URL's:

```csharp
    public class Clients
    {
        public string ProducerClientUrl { get; set; }
    }
```

    Last piece of the puzzle is the config section in either **\*.json** or **\*.{ENV}.json** files:

```json
    {
        "Clients": {
            "ProducerClientUrl": "http://company.com/api"
        }
    }
```

As mentioned above, in order to use the registered API Client, we need to inject it as in example below:

```csharp
public class MyService
{
    private readonly IValuesClient _valuesClient;

    public MyService(IValuesClient valuesClient)
    {
        if (valuesClient == null) throw new ArgumentNullException(nameof(valuesClient));
        _valuesClient = valuesClient;
    }

    public async Task<IEnumerable<string>> GetValues()
    {
        return await _valuesClient.GetValues();
    }
}
```

[Back to Index](https://github.com/icgam/ICG.DotNetCore.Framework)
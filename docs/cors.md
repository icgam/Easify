## Cross-Origin Resource Sharing
Browser security prevents a web page from making AJAX requests to another domain. This restriction is called the same-origin policy, and prevents a malicious site from reading sensitive data from another site. 

However, sometimes you might want to let other sites make cross-origin requests to your web API. It is really important when we separate website from the dependent API(s).

Cross Origin Resource Sharing (CORS) is a W3C standard that allows a server to relax the same-origin policy. Using CORS, a server can explicitly allow some cross-origin requests while rejecting others. NET Core application.

### Useful links
* [Cors Definition](https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS)
* [Cors in ASP.net Core](https://docs.microsoft.com/en-us/aspnet/core/security/cors)

The implementation in the framework is the minimalistic approach with the default policy which can be extended in a later stage with a proper configuration. Here is the list of the extension methods and the default one has already been added to the default bootstrap for web api projects:

```csharp
  AddCorsWithDefaultPolicy(this IServiceCollection services, Action<CorsPolicyBuilder> configure)
  AddCorsWithDefaultPolicy(this IServiceCollection services)
  UseCorsWithDefaultPolicy(this IAppBuilder app)
```

## Core Interfaces ([Back to Index](https://github.com/icgam/ICG.DotNetCore.Framework))

Idea behind core services is to encapsulate interfaces that are highly reusable across application, without taking dependencies on heavy external frameworks.

Currently we have following highly reusable services:

* Mapper service

```csharp
    public interface ITypeMapper
    {
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
        TDestination Map<TSource, TDestination>(TSource source);
        TDestination Map<TDestination>(object source);
    }
```

* Runtime factory / Resolver service:

```csharp
    public interface IComponentResolver
    {
        bool IsRegistered<TComponent>() where TComponent : class;
        bool IsRegistered(Type type);
        IEnumerable<TComponent> Resolve<TComponent>() where TComponent : class;
        IEnumerable<object> Resolve(Type type);
    }
```

* REST client interface (carries through Correlation-ID):

```csharp
    public interface IRestClient
    {
        [Header(HttpHeaders.HttpRequestId)]
        string CorrelationId { get; set; }
    }
```

* Date & Time service (allows mocking of date & time where needed):

```csharp
    public interface IDateTimeProvider
    {
        DateTime Now();
    }
```

* Circular Buffer:

```csharp
    public interface ICircularBuffer<T> : IEnumerable<T>
    {
    }
```

[Back to Index](https://github.com/icgam/ICG.DotNetCore.Framework)

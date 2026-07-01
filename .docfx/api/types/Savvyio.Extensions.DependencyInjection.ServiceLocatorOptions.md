---
uid: Savvyio.Extensions.DependencyInjection.ServiceLocatorOptions
example:
- *content
---
Configure a `ServiceLocatorOptions` instance to provide a custom `IServiceLocator` factory, passed to `AddServiceLocator`.

```csharp
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Dispatchers;
using Savvyio.Extensions.DependencyInjection;

namespace ExampleApp;

public static class ServiceLocatorSetup
{
    public static IServiceCollection AddLocator(this IServiceCollection services)
    {
        services.AddServiceLocator(ConfigureLocator);
        return services;
    }

    private static void ConfigureLocator(ServiceLocatorOptions options)
    {
        options.Lifetime = ServiceLifetime.Singleton;
        options.ImplementationFactory = provider => new ServiceLocator(provider.GetServices);
    }
}
```


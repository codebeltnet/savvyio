---
uid: Savvyio.Extensions.DependencyInjection.SavvyioDependencyInjectionOptions
example:
- *content
---
Configure a `SavvyioDependencyInjectionOptions` instance to control assembly scanning, handler and dispatcher discovery, and DI service lifetimes.

```csharp
using Microsoft.Extensions.DependencyInjection;
using Savvyio;
using Savvyio.Extensions.DependencyInjection;

namespace ExampleApp;

public static class DependencyInjectionSetup
{
    public static IServiceCollection AddApplicationMessaging(this IServiceCollection services)
    {
        services.AddSavvyIO(ConfigureOptions);
        services.AddHandlerServicesDescriptor();
        return services;
    }

    private static void ConfigureOptions(SavvyioDependencyInjectionOptions options)
    {
        options.AddAssemblyRangeToScan(typeof(SavvyioOptions).Assembly, typeof(DependencyInjectionSetup).Assembly);
        options.EnableDispatcherDiscovery();
        options.EnableHandlerDiscovery();
        options.EnableHandlerServicesDescriptor();
        options.ServiceLocatorLifetime = ServiceLifetime.Singleton;
        options.HandlerServicesLifetime = ServiceLifetime.Scoped;
        options.DispatcherServicesLifetime = ServiceLifetime.Singleton;
    }
}
```


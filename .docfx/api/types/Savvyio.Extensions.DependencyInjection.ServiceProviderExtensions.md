---
uid: Savvyio.Extensions.DependencyInjection.ServiceProviderExtensions
example:
- *content
---
`WriteHandlerDiscoveriesToLog<TCategoryName>` resolves `IHandlerServicesDescriptor` from the service provider and writes the handler discovery report to the named logger. Call it at startup after building the service provider.

```csharp
using System;
using Microsoft.Extensions.DependencyInjection;
using Savvyio;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Handlers;

namespace ExampleApp;

public static class DependencyInjectionDiagnostics
{
    public static IServiceProvider BuildProvider()
    {
        var services = new ServiceCollection();
        services.AddSavvyIO(options => options.EnableHandlerServicesDescriptor());
        services.AddHandlerServicesDescriptor();
        return services.BuildServiceProvider();
    }

    public static void LogRegisteredHandlers(IServiceProvider provider)
    {
        provider.WriteHandlerDiscoveriesToLog<HandlerDiscoveryLogCategory>();
    }
}

public sealed class HandlerDiscoveryLogCategory { }
```


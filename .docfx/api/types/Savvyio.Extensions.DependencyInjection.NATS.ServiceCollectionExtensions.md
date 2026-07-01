---
uid: Savvyio.Extensions.DependencyInjection.NATS.ServiceCollectionExtensions
example:
- *content
---
Register NATS command queues and event buses in the DI container with `AddNatsCommandQueue` and `AddNatsEventBus`. Both methods configure NATS JetStream settings and register the concrete queue/bus types.

```csharp
using System;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.NATS;

namespace ExampleApp;

public static class NatsRegistration
{
    public static IServiceCollection Configure(IServiceCollection services)
    {
        services.AddSavvyIO();
        services.AddNatsCommandQueue(options =>
        {
            options.NatsUrl = new Uri("nats://localhost:4222");
            options.Subject = "account-commands";
            options.StreamName = "savvyio-commands";
        });
        services.AddNatsEventBus(options =>
        {
            options.NatsUrl = new Uri("nats://localhost:4222");
            options.Subject = "account-events";
        });
        return services;
    }
}
```

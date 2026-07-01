---
uid: Savvyio.Extensions.DependencyInjection.NATS.EventDriven.NatsEventBus`1
example:
- *content
---
`NatsEventBus<TMarker>` is the DI-registered NATS event bus with a lifetime marker. Register it with `AddNatsEventBus` and resolve it as `NatsEventBus` or `IPublishSubscribeChannel<IIntegrationEvent>`.

```csharp
using System;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.EventDriven;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.NATS;
using Savvyio.Extensions.NATS.EventDriven;
using Savvyio.Messaging;

namespace ExampleApp;

public static class NatsBusUsage
{
    public static NatsEventBus GetBus(IServiceProvider provider)
    {
        return provider.GetRequiredService<NatsEventBus>();
    }

    public static IPublishSubscribeChannel<IIntegrationEvent> GetChannel(IServiceProvider provider)
    {
        return provider.GetRequiredService<IPublishSubscribeChannel<IIntegrationEvent>>();
    }

    public static IServiceCollection Register(IServiceCollection services)
    {
        services.AddSavvyIO();
        services.AddNatsEventBus(o => { o.NatsUrl = new Uri("nats://localhost:4222"); o.Subject = "events"; });
        return services;
    }
}
```



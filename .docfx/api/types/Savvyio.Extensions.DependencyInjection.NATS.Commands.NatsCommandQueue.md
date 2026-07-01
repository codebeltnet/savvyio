---
uid: Savvyio.Extensions.DependencyInjection.NATS.Commands.NatsCommandQueue`1
example:
- *content
---
`NatsCommandQueue<TMarker>` is the DI-registered NATS command queue with a lifetime marker. Register it with `AddNatsCommandQueue` and resolve it as `NatsCommandQueue` or `IPointToPointChannel<ICommand>`.

```csharp
using System;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Commands;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.NATS;
using Savvyio.Extensions.NATS.Commands;
using Savvyio.Messaging;

namespace ExampleApp;

public static class NatsQueueUsage
{
    public static NatsCommandQueue GetQueue(IServiceProvider provider)
    {
        return provider.GetRequiredService<NatsCommandQueue>();
    }

    public static IPointToPointChannel<ICommand> GetChannel(IServiceProvider provider)
    {
        return provider.GetRequiredService<IPointToPointChannel<ICommand>>();
    }

    public static IServiceCollection Register(IServiceCollection services)
    {
        services.AddSavvyIO();
        services.AddNatsCommandQueue(o => { o.Subject = "commands"; });
        return services;
    }
}
```


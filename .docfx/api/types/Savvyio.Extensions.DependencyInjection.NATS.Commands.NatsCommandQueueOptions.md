---
uid: Savvyio.Extensions.DependencyInjection.NATS.Commands.NatsCommandQueueOptions`1
example:
- *content
---
`NatsCommandQueueOptions<TMarker>` carries the NATS JetStream stream, consumer, subject, and connection settings for a DI-registered NATS command queue.

```csharp
using System;
using Savvyio.Extensions.NATS.Commands;

namespace ExampleApp;

public class NatsQueueOptionsExample
{
    public static NatsCommandQueueOptions CreateOptions()
    {
        return new NatsCommandQueueOptions
        {
            NatsUrl = new Uri("nats://localhost:4222"),
            Subject = "account-commands",
            StreamName = "savvyio-commands",
            ConsumerName = "account-consumer"
        };
    }
}
```

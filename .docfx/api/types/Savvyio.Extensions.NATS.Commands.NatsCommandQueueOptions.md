---
uid: Savvyio.Extensions.NATS.Commands.NatsCommandQueueOptions
example:
- *content
---
Configure a `NatsCommandQueueOptions` with the NATS JetStream stream, consumer, and subject settings required for command queue delivery.

```csharp
using System;
using Savvyio.Extensions.NATS.Commands;

namespace ExampleApp;

public class NatsCommandSetup
{
    public static NatsCommandQueueOptions CreateOptions()
    {
        var options = new NatsCommandQueueOptions
        {
            NatsUrl = new Uri("nats://localhost:4222"),
            Subject = "commands",
            StreamName = "savvyio-commands",
            ConsumerName = "account-command-consumer",
            AutoAcknowledge = true
        };
        return options;
    }
}
```

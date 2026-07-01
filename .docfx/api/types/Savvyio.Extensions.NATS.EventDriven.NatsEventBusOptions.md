---
uid: Savvyio.Extensions.NATS.EventDriven.NatsEventBusOptions
example:
- *content
---
Configure a `NatsEventBusOptions` with the NATS server URL and subject settings required for publishing and subscribing to integration events.

```csharp
using System;
using Savvyio.Extensions.NATS.EventDriven;

namespace ExampleApp;

public class NatsEventBusSetup
{
    public static NatsEventBusOptions CreateOptions()
    {
        var options = new NatsEventBusOptions
        {
            NatsUrl = new Uri("nats://localhost:4222"),
            Subject = "integration-events"
        };
        return options;
    }
}
```


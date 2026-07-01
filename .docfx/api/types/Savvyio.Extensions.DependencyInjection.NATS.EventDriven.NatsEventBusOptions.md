---
uid: Savvyio.Extensions.DependencyInjection.NATS.EventDriven.NatsEventBusOptions`1
example:
- *content
---
`NatsEventBusOptions` (or its DI generic variant) carries the NATS server URL and subject settings used when `AddNatsEventBus` registers the NATS event bus in DI.

```csharp
using System;
using Savvyio.Extensions.NATS.EventDriven;

namespace ExampleApp;

public class NatsEventBusOptionsExample
{
    public static NatsEventBusOptions CreateOptions()
    {
        return new NatsEventBusOptions
        {
            NatsUrl = new Uri("nats://localhost:4222"),
            Subject = "events.account"
        };
    }
}
```


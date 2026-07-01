---
uid: Savvyio.Extensions.DependencyInjection.RabbitMQ.EventDriven.RabbitMqEventBusOptions`1
example:
- *content
---
`RabbitMqEventBusOptions` carries the AMQP URL and exchange settings used when `AddRabbitMqEventBus` registers the RabbitMQ event bus in DI.

```csharp
using System;
using Savvyio.Extensions.RabbitMQ.EventDriven;

namespace ExampleApp;

public class RabbitMqEventBusOptionsExample
{
    public static RabbitMqEventBusOptions CreateOptions()
    {
        return new RabbitMqEventBusOptions
        {
            AmqpUrl = new Uri("amqp://guest:guest@localhost:5672"),
            ExchangeName = "account-events",
            Persistent = true
        };
    }
}
```

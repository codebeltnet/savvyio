---
uid: Savvyio.Extensions.RabbitMQ.EventDriven.RabbitMqEventBusOptions
example:
- *content
---
Configure a `RabbitMqEventBusOptions` with the AMQP URL and exchange settings required for RabbitMQ event publishing and subscription.

```csharp
using System;
using Savvyio.Extensions.RabbitMQ.EventDriven;

namespace ExampleApp;

public class RabbitMqEventSetup
{
    public static RabbitMqEventBusOptions CreateOptions()
    {
        var options = new RabbitMqEventBusOptions
        {
            AmqpUrl = new Uri("amqp://guest:guest@localhost:5672"),
            ExchangeName = "account-events",
            Persistent = true
        };
        return options;
    }
}
```

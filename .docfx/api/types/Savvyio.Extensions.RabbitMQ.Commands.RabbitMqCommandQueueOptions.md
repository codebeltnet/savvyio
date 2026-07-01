---
uid: Savvyio.Extensions.RabbitMQ.Commands.RabbitMqCommandQueueOptions
example:
- *content
---
Configure a `RabbitMqCommandQueueOptions` with the AMQP URL and queue settings required for durable RabbitMQ command delivery.

```csharp
using System;
using Savvyio.Extensions.RabbitMQ.Commands;

namespace ExampleApp;

public class RabbitMqCommandSetup
{
    public static RabbitMqCommandQueueOptions CreateOptions()
    {
        var options = new RabbitMqCommandQueueOptions
        {
            AmqpUrl = new Uri("amqp://guest:guest@localhost:5672"),
            QueueName = "account-commands",
            Durable = true,
            Persistent = true,
            AutoAcknowledge = false
        };
        return options;
    }
}
```

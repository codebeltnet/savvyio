---
uid: Savvyio.Extensions.DependencyInjection.RabbitMQ.Commands.RabbitMqCommandQueueOptions`1
example:
- *content
---
`RabbitMqCommandQueueOptions` carries the AMQP URL and queue settings used when `AddRabbitMqCommandQueue` registers the RabbitMQ command queue in DI.

```csharp
using System;
using Savvyio.Extensions.RabbitMQ.Commands;

namespace ExampleApp;

public class RabbitMqCommandQueueOptionsExample
{
    public static RabbitMqCommandQueueOptions CreateOptions()
    {
        return new RabbitMqCommandQueueOptions
        {
            AmqpUrl = new Uri("amqp://guest:guest@localhost:5672"),
            QueueName = "account-commands",
            Durable = true,
            Persistent = true,
            AutoAcknowledge = false
        };
    }
}
```

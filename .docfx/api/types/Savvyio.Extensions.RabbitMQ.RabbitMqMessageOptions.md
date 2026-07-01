---
uid: Savvyio.Extensions.RabbitMQ.RabbitMqMessageOptions
example:
- *content
---
Configure a `RabbitMqMessageOptions` with the AMQP URL and message persistence settings shared by all RabbitMQ message types in Savvy I/O.

```csharp
using System;
using Savvyio.Extensions.RabbitMQ;

namespace ExampleApp;

public class RabbitMqSetup
{
    public static RabbitMqMessageOptions CreateOptions()
    {
        var options = new RabbitMqMessageOptions
        {
            AmqpUrl = new Uri("amqp://guest:guest@localhost:5672"),
            Persistent = true
        };
        return options;
    }
}
```

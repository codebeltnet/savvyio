---
uid: Savvyio.Extensions.RabbitMQ.Commands.RabbitMqCommandQueue
example:
- *content
---
`RabbitMqCommandQueue` publishes and receives commands through RabbitMQ. Configure it with `RabbitMqCommandQueueOptions` and use it as `IPointToPointChannel<ICommand>`.

```csharp
using System;
using Savvyio.Commands;
using Savvyio.Extensions.RabbitMQ.Commands;
using Savvyio.Messaging;

namespace ExampleApp;

public class RabbitMqCommandQueueConfig
{
    public static RabbitMqCommandQueueOptions CreateOptions()
    {
        return new RabbitMqCommandQueueOptions { AmqpUrl = new Uri("amqp://guest:guest@localhost:5672"), QueueName = "account-commands", Durable = true };
    }

    public static IPointToPointChannel<ICommand> AsChannel(RabbitMqCommandQueue queue) => queue;
}
```

---
uid: Savvyio.Extensions.DependencyInjection.RabbitMQ.Commands.RabbitMqCommandQueue`1
example:
- *content
---
The `RabbitMqCommandQueue` registered by `AddRabbitMqCommandQueue` is the concrete DI-managed command queue; resolve it to access `SendAsync` and `ReceiveAsync`.

```csharp
using System;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Commands;
using Savvyio.Extensions.RabbitMQ.Commands;
using Savvyio.Messaging;

namespace ExampleApp;

public class RabbitMqCommandQueueUsage
{
    private readonly RabbitMqCommandQueue _queue;

    public RabbitMqCommandQueueUsage(RabbitMqCommandQueue queue)
    {
        _queue = queue;
    }

    public IPointToPointChannel<ICommand> AsChannel() => _queue;
}
```

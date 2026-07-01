---
uid: Savvyio.Extensions.DependencyInjection.RabbitMQ.EventDriven.RabbitMqEventBus`1
example:
- *content
---
The `RabbitMqEventBus` registered by `AddRabbitMqEventBus` is the concrete DI-managed event bus; resolve it to access `PublishAsync` and `SubscribeAsync`.

```csharp
using System;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.EventDriven;
using Savvyio.Extensions.RabbitMQ.EventDriven;
using Savvyio.Messaging;

namespace ExampleApp;

public class RabbitMqEventBusUsage
{
    private readonly RabbitMqEventBus _bus;

    public RabbitMqEventBusUsage(RabbitMqEventBus bus)
    {
        _bus = bus;
    }

    public IPublishSubscribeChannel<IIntegrationEvent> AsChannel() => _bus;
}
```

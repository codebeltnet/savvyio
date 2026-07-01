---
uid: Savvyio.Extensions.RabbitMQ.EventDriven.RabbitMqEventBus
example:
- *content
---
`RabbitMqEventBus` publishes and subscribes to integration events through RabbitMQ. Configure it with `RabbitMqEventBusOptions` and use it as `IPublishSubscribeChannel<IIntegrationEvent>`.

```csharp
using System;
using Savvyio.EventDriven;
using Savvyio.Extensions.RabbitMQ.EventDriven;
using Savvyio.Messaging;

namespace ExampleApp;

public class RabbitMqEventBusConfig
{
    public static RabbitMqEventBusOptions CreateOptions() => new RabbitMqEventBusOptions { AmqpUrl = new Uri("amqp://guest:guest@localhost:5672"), ExchangeName = "events" };
    public static IPublishSubscribeChannel<IIntegrationEvent> AsChannel(RabbitMqEventBus bus) => bus;
}
```

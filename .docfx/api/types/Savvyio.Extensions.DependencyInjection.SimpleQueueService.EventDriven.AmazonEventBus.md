---
uid: Savvyio.Extensions.DependencyInjection.SimpleQueueService.EventDriven.AmazonEventBus`1
example:
- *content
---
`AmazonEventBus<TMarker>` is the DI-registered Amazon SNS/SQS event bus with a lifetime marker. Inject it as `IPublishSubscribeChannel<IIntegrationEvent>` to publish events to an SNS topic and receive them via SQS.

```csharp
using System;
using System.Threading.Tasks;
using Savvyio.EventDriven;
using Savvyio.Extensions.SimpleQueueService.EventDriven;
using Savvyio.Messaging;

namespace ExampleApp;

public sealed class SnsEventPublisher
{
    private readonly IPublishSubscribeChannel<IIntegrationEvent> _bus;

    public SnsEventPublisher(AmazonEventBus bus)
    {
        _bus = bus;
    }

    public Task PublishAsync(IMessage<IIntegrationEvent> message)
    {
        return _bus.PublishAsync(message);
    }
}
```


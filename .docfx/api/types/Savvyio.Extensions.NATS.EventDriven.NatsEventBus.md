---
uid: Savvyio.Extensions.NATS.EventDriven.NatsEventBus
example:
- *content
---
`NatsEventBus` is the NATS JetStream event bus that publishes and subscribes to `IMessage<IIntegrationEvent>` envelopes. Configure it with `NatsEventBusOptions` and `IMarshaller`.

```csharp
using System;
using Savvyio.EventDriven;
using Savvyio.Extensions.NATS.EventDriven;
using Savvyio.Messaging;

namespace ExampleApp;

public class NatsEventBusConfig
{
    public static NatsEventBusOptions CreateOptions()
    {
        return new NatsEventBusOptions
        {
            NatsUrl = new Uri("nats://localhost:4222"),
            Subject = "integration-events"
        };
    }

    public static IPublishSubscribeChannel<IIntegrationEvent> AsChannel(NatsEventBus bus) => bus;
}
```




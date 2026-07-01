---
uid: Savvyio.Extensions.NATS.Commands.NatsCommandQueue
example:
- *content
---
`NatsCommandQueue` is the NATS JetStream command queue that publishes and receives `IMessage<ICommand>` envelopes. Configure it with `NatsCommandQueueOptions` and `IMarshaller` to set up the connection.

```csharp
using System;
using Savvyio.Commands;
using Savvyio.Extensions.NATS.Commands;
using Savvyio.Messaging;

namespace ExampleApp;

public class NatsCommandQueueConfig
{
    public static NatsCommandQueueOptions CreateOptions()
    {
        return new NatsCommandQueueOptions
        {
            NatsUrl = new Uri("nats://localhost:4222"),
            Subject = "account-commands",
            StreamName = "savvyio-commands",
            ConsumerName = "account-command-consumer",
            AutoAcknowledge = true
        };
    }

    public static IPointToPointChannel<ICommand> AsChannel(NatsCommandQueue queue) => queue;
}
```




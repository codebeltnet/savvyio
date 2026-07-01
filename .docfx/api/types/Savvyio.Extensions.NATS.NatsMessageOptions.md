---
uid: Savvyio.Extensions.NATS.NatsMessageOptions
example:
- *content
---
Configure a `NatsMessageOptions` to point at a NATS server and specify the subject for message delivery.

```csharp
using System;
using Savvyio.Extensions.NATS;

namespace ExampleApp;

public class NatsSetup
{
    public static NatsMessageOptions CreateOptions()
    {
        var options = new NatsMessageOptions
        {
            NatsUrl = new Uri("nats://localhost:4222"),
            Subject = "account-commands"
        };
        return options;
    }
}
```

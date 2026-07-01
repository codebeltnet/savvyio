---
uid: Savvyio.Messaging.Message`1
example:
- *content
---
Wrap a request in a `Message<T>` envelope to prepare it for transport to another subsystem. The envelope pairs the payload with a source URI, a type discriminator, and a unique ID.

```csharp
using System;
using Savvyio;
using Savvyio.Messaging;

namespace ExampleApp;

public sealed class MessageExample
{
    public void Send()
    {
        var message = new Message<CreateOrderCommand>(
            "msg-42",
            new Uri("urn:orders"),
            "orders.create",
            new CreateOrderCommand("ORD-42"));

        Console.WriteLine($"Sending message {message.Id} from {message.Source} with type {message.Type}");
    }
}

public sealed record CreateOrderCommand(string OrderId) : Request;
```


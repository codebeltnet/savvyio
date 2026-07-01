---
uid: Savvyio.Messaging.MessageExtensions
example:
- *content
---
This example shows how to clone a message envelope before adding transport-specific state to the copy.

```csharp
using System;
using Savvyio;
using Savvyio.Messaging;

namespace ExampleApp;

public sealed class MessageExtensionsExample
{
    public IMessage<CreateOrderCommand> Clone()
    {
        var original = new Message<CreateOrderCommand>("msg-42", new Uri("urn:orders"), "orders.created", new CreateOrderCommand("ORD-42"));
        return original.Clone();
    }
}

public sealed record CreateOrderCommand(string OrderId) : Request;
```

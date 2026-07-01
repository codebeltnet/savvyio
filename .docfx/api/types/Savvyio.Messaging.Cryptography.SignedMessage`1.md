---
uid: Savvyio.Messaging.Cryptography.SignedMessage`1
example:
- *content
---
Attach a signature to an existing `IMessage<T>` envelope using `MessageExtensions.Sign<T>`. `SignedMessage<T>` carries both the original message and the computed signature so the consumer can verify integrity before dispatching.

```csharp
using System;
using Savvyio;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;

namespace ExampleApp;

public sealed class SignedMessageExample
{
    public void SignAndVerify()
    {
        var message = new Message<CreateOrderCommand>(
            "msg-42",
            new Uri("urn:orders"),
            "orders.create",
            new CreateOrderCommand("ORD-42"));

        var signed = new SignedMessage<CreateOrderCommand>(message, "hmac-signature-value");
        Console.WriteLine($"Message {signed.Id} signed with: {signed.Signature}");
    }
}

public sealed record CreateOrderCommand(string OrderId) : Request;
```


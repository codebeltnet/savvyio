---
uid: Savvyio.EventDriven.Messaging.CloudEvents.Cryptography.SignedCloudEvent`1
example:
- *content
---
This example shows how to wrap a CloudEvent together with the signature that protects its serialized envelope.

```csharp
using System;
using Savvyio;
using Savvyio.EventDriven;
using Savvyio.EventDriven.Messaging.CloudEvents;
using Savvyio.EventDriven.Messaging.CloudEvents.Cryptography;
using Savvyio.Messaging;

namespace ExampleApp;

public sealed class SignedCloudEventExample
{
    public ISignedCloudEvent<MemberCreatedEvent> Create()
    {
        var message = new Message<MemberCreatedEvent>("msg-42", new Uri("https://api.example.com/members"), "members.created", new MemberCreatedEvent("MEM-42"));
        var cloudEvent = message.ToCloudEvent();
        return new SignedCloudEvent<MemberCreatedEvent>(cloudEvent, "signature-value");
    }
}

public sealed record MemberCreatedEvent(string MemberId) : Request, IIntegrationEvent;
```

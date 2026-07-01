---
uid: Savvyio.EventDriven.Messaging.CloudEvents.MessageExtensions
example:
- *content
---
This example shows how to convert a message envelope into a CloudEvents-compliant envelope before publishing it externally.

```csharp
using System;
using Savvyio;
using Savvyio.EventDriven;
using Savvyio.EventDriven.Messaging.CloudEvents;
using Savvyio.Messaging;

namespace ExampleApp;

public sealed class CloudEventMessageExtensionsExample
{
    public ICloudEvent<MemberCreatedEvent> Convert()
    {
        var message = new Message<MemberCreatedEvent>("msg-42", new Uri("https://api.example.com/members"), "members.created", new MemberCreatedEvent("MEM-42"));
        return message.ToCloudEvent();
    }
}

public sealed record MemberCreatedEvent(string MemberId) : Request, IIntegrationEvent;
```

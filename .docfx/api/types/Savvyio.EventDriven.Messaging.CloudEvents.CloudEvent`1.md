---
uid: Savvyio.EventDriven.Messaging.CloudEvents.CloudEvent`1
example:
- *content
---
This example shows how to convert a transport message into a concrete CloudEvent envelope and add extension attributes before publishing it. The explicit CloudEvent<T> type is then available for downstream inspection of specversion, payload, and custom partition metadata.

```csharp
using System;
using Savvyio;
using Savvyio.EventDriven;
using Savvyio.EventDriven.Messaging.CloudEvents;
using Savvyio.Messaging;

namespace ExampleApp;

public sealed class CloudEventExample
{
    public string Create()
    {
        var message = new Message<MemberCreatedEvent>("msg-42", new Uri("https://api.example.com/members"), "members.created", new MemberCreatedEvent("MEM-42"));
        var cloudEvent = (CloudEvent<MemberCreatedEvent>)message.ToCloudEvent();
        cloudEvent["partitionkey"] = "members";
        return $"{cloudEvent.Specversion}:{cloudEvent["partitionkey"]}";
    }
}

public sealed record MemberCreatedEvent(string MemberId) : Request, IIntegrationEvent;
```

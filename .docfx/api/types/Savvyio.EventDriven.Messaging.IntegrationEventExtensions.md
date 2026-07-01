---
uid: Savvyio.EventDriven.Messaging.IntegrationEventExtensions
example:
- *content
---
The following example shows how to wrap an integration event in a transport-friendly `Message<T>` by calling `ToMessage`.

```csharp
using System;
using Savvyio.EventDriven;
using Savvyio.EventDriven.Messaging;
using Savvyio.Messaging;

namespace ExampleApp.IntegrationMessages;

public sealed class IntegrationEventMessagingExtensionsUsage
{
    public IntegrationEventMessagingExtensionsUsage()
    {
        var integrationEvent = new MemberWelcomeEmailQueued("member-42");
        IMessage<MemberWelcomeEmailQueued> message = integrationEvent.ToMessage(
            new Uri("https://api.example.com/messages/member-42"),
            nameof(MemberWelcomeEmailQueued),
            options => options.MessageId = "msg-member-42");

        MessageId = message.Id;
        Type = message.Type;
        Source = message.Source;
    }

    public string MessageId { get; }

    public string Type { get; }

    public string Source { get; }
}

public sealed record MemberWelcomeEmailQueued(string MemberId) : IntegrationEvent;
```

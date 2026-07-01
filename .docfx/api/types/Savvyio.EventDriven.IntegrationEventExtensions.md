---
uid: Savvyio.EventDriven.IntegrationEventExtensions
example:
- *content
---
Use `IntegrationEventExtensions` to read the event ID, timestamp, and member type from an integration event's metadata before publishing it to another subsystem.

```csharp
using System;
using Savvyio.EventDriven;

namespace ExampleApp;

public sealed class IntegrationEventExtensionsExample
{
    public (string EventId, DateTime Timestamp, string MemberType) Describe()
    {
        var e = new MemberCreatedEvent("MEM-42");
        e.Metadata[Savvyio.MetadataDictionary.MemberType] = typeof(MemberCreatedEvent).FullName;

        string eventId = e.GetEventId<MemberCreatedEvent>();
        DateTime timestamp = e.GetTimestamp<MemberCreatedEvent>();
        string memberType = e.GetMemberType<MemberCreatedEvent>();
        Console.WriteLine($"Event {eventId} at {timestamp:O}, type: {memberType}");
        return (eventId, timestamp, memberType);
    }
}

public sealed record MemberCreatedEvent(string MemberId) : IntegrationEvent;
```



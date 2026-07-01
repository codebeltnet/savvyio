---
uid: Savvyio.Domain.DomainEventExtensions
example:
- *content
---
This example shows how to stamp a domain event with envelope metadata and read the event identifier and timestamp later.

```csharp
using System;
using Savvyio;
using Savvyio.Domain;

namespace ExampleApp;

public sealed class DomainEventExtensionsExample
{
    public (string EventId, DateTime Timestamp) Describe()
    {
        var e = new AccountOpenedEvent("ACC-42").SetEventId("evt-42").SetTimestamp(new DateTime(2026,7,1,0,0,0,DateTimeKind.Utc));
        return (e.GetEventId(), e.GetTimestamp());
    }
}

public sealed record AccountOpenedEvent(string AccountId) : Request, IDomainEvent;
```

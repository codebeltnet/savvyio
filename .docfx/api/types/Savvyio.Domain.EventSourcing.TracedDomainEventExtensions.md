---
uid: Savvyio.Domain.EventSourcing.TracedDomainEventExtensions
example:
- *content
---
Use `TracedDomainEventExtensions` to set and read back the aggregate version and member type on a `ITracedDomainEvent`. Call `SetAggregateVersion` when recording the event and `GetAggregateVersion` when replaying the aggregate stream.

```csharp
using System;
using Savvyio.Domain.EventSourcing;

namespace ExampleApp;

public sealed class TracedDomainEventExtensionsExample
{
    public (long Version, string MemberType) StampAndRead()
    {
        var e = new AccountStateCapturedEvent("ACC-42")
            .SetAggregateVersion(7);
        e.Metadata[Savvyio.MetadataDictionary.MemberType] = typeof(AccountStateCapturedEvent).FullName;

        long version = e.GetAggregateVersion<AccountStateCapturedEvent>();
        string memberType = e.GetMemberType<AccountStateCapturedEvent>();
        Console.WriteLine($"Version: {version}, MemberType: {memberType}");
        return (version, memberType);
    }
}

public sealed record AccountStateCapturedEvent(string AccountId) : Savvyio.Request, ITracedDomainEvent;
```



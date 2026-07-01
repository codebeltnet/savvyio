---
uid: Savvyio.Extensions.EFCore.Domain.EventSourcing.EfCoreTracedAggregateEntity`2
example:
- *content
---
`EfCoreTracedAggregateEntity<TEntity, TKey>` is the EF Core entity class that stores one traced domain event row in the event-store table. Each row carries the aggregate ID, version, event type, and serialized event payload. The example creates a traced entity directly from an aggregate and a domain event, then reads back the stored aggregate ID and version.

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Savvyio;
using Savvyio.Domain.EventSourcing;
using Savvyio.Extensions.EFCore.Domain.EventSourcing;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class EventRecordExample
{
    public EfCoreTracedAggregateEntity<OrderTimeline, Guid> CreateRecord()
    {
        var aggregate = new OrderTimeline(Guid.NewGuid(), "PO-6001");
        var domainEvent = aggregate.Events.Single();

        return new EfCoreTracedAggregateEntity<OrderTimeline, Guid>(aggregate, domainEvent, new SimpleMarshaller());
    }
}

public sealed class OrderTimeline : TracedAggregateRoot<Guid>
{
    public OrderTimeline(Guid id, string orderNumber) : base()
    {
        AddEvent(new OrderPlaced(id, orderNumber));
    }

    private OrderTimeline(Guid id, IEnumerable<ITracedDomainEvent> events) : base(id, events)
    {
    }

    public string OrderNumber { get; private set; } = string.Empty;

    protected override void RegisterDelegates(IFireForgetRegistry<ITracedDomainEvent> handler)
    {
        handler.Register<OrderPlaced>(e =>
        {
            Id = e.OrderId;
            OrderNumber = e.OrderNumber;
        });
    }
}

public sealed record OrderPlaced(Guid OrderId, string OrderNumber) : TracedDomainEvent;

public sealed class SimpleMarshaller : IMarshaller
{
    public Stream Serialize<TValue>(TValue value)
    {
        return new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(value));
    }

    public Stream Serialize(object value, Type inputType)
    {
        return new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(value, inputType));
    }

    public TValue Deserialize<TValue>(Stream data)
    {
        return JsonSerializer.Deserialize<TValue>(data)!;
    }

    public object Deserialize(Stream data, Type returnType)
    {
        return JsonSerializer.Deserialize(data, returnType)!;
    }
}
```

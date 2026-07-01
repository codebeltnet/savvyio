---
uid: Savvyio.Extensions.EFCore.Domain.EventSourcing.EfCoreTracedAggregateEntityExtensions
example:
- *content
---
`EfCoreTracedAggregateEntityExtensions.ToTracedDomainEvent<TEntity, TKey>` rehydrates an `EfCoreTracedAggregateEntity<TEntity, TKey>` row back into the corresponding `ITracedDomainEvent` by deserializing the payload with the registered marshaller. This is called internally by `EfCoreTracedAggregateRepository` when loading aggregate history. The example creates a traced entity, calls `ToTracedDomainEvent`, and verifies the deserialized event matches the original.

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

public sealed class EventHydrationExample
{
    public OrderPlaced Rehydrate()
    {
        var aggregate = new OrderTimeline(Guid.NewGuid(), "PO-7001");
        var marshaller = new SimpleMarshaller();
        var entity = new EfCoreTracedAggregateEntity<OrderTimeline, Guid>(aggregate, aggregate.Events.Single(), marshaller);

        return (OrderPlaced)entity.ToTracedDomainEvent(typeof(OrderPlaced), marshaller);
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

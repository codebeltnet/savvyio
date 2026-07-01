---
uid: Savvyio.Extensions.EFCore.Domain.EventSourcing.EfCoreTracedAggregateRepository`2
example:
- *content
---
`EfCoreTracedAggregateRepository<TAggregateRoot, TKey, TContext>` stores and loads event-sourced aggregates by writing and reading individual traced domain event rows. The setup requires a context configured with `ModelBuilder.AddEventSourcing`, an `IMarshaller` for event serialization, and the aggregate type with `RegisterDelegates` implemented. The example creates a repository, appends an event, and rehydrates the aggregate from stored events.

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Savvyio;
using Savvyio.Domain.EventSourcing;
using Savvyio.Extensions.EFCore;
using Savvyio.Extensions.EFCore.Domain.EventSourcing;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class EventSourcingWorkflow
{
    public Task<OrderTimeline> LoadAsync(Guid id)
    {
        var source = new EfCoreDataSource(new EfCoreDataSourceOptions
        {
            ContextConfigurator = builder => builder.EnableDetailedErrors(),
            ModelConstructor = modelBuilder => modelBuilder.AddEventSourcing<OrderTimeline, Guid>()
        });

        var repository = new EfCoreTracedAggregateRepository<OrderTimeline, Guid>(source, new SimpleMarshaller());
        repository.Add(new OrderTimeline(id, "PO-8001"));

        return repository.GetByIdAsync(id);
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

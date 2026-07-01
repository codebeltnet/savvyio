---
uid: Savvyio.Extensions.EFCore.Domain.EventSourcing.ModelBuilderExtensions
example:
- *content
---
This example shows how `AddEventSourcing` can be applied in `OnModelCreating` to register the event table for a traced aggregate.

```csharp
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Savvyio.Domain.EventSourcing;
using Savvyio.Extensions.EFCore;
using Savvyio.Extensions.EFCore.Domain.EventSourcing;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class EventStoreContextFactory
{
    public EventStoreContext Create()
    {
        return new EventStoreContext(new EfCoreDataSourceOptions
        {
            ContextConfigurator = builder => builder.EnableDetailedErrors()
        });
    }
}

public sealed class EventStoreContext : EfCoreDbContext
{
    public EventStoreContext(EfCoreDataSourceOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.AddEventSourcing<OrderTimeline, Guid>(options => options.TableName = "OrderTimelineEvents");
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

    protected override void RegisterDelegates(IFireForgetRegistry<ITracedDomainEvent> handler)
    {
        handler.Register<OrderPlaced>(_ => { });
    }
}

public sealed record OrderPlaced(Guid OrderId, string OrderNumber) : TracedDomainEvent;
```

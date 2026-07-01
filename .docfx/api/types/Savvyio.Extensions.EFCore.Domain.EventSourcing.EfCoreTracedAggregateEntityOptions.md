---
uid: Savvyio.Extensions.EFCore.Domain.EventSourcing.EfCoreTracedAggregateEntityOptions
example:
- *content
---
``EfCoreTracedAggregateEntityOptions`` customizes the event-store table name and column names used by ``ModelBuilderExtensions.AddEventSourcing``. Configure it inside the setup lambda to override the defaults before EF Core creates the migration schema. The example applies custom column names and prints them to verify the configured values.

```csharp
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Savvyio.Domain.EventSourcing;
using Savvyio.Extensions.EFCore.Domain.EventSourcing;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class EventSchemaConfiguration
{
    public ModelBuilder Configure(ModelBuilder modelBuilder)
    {
        return modelBuilder.AddEventSourcing<OrderTimeline, Guid>(ConfigureOptions);
    }

    private static void ConfigureOptions(EfCoreTracedAggregateEntityOptions options)
    {
        options.TableName = "OrderTimelineEvents";
        options.CompositePrimaryKeyIdColumnName = "order_id";
        options.CompositePrimaryKeyVersionColumnName = "aggregate_version";
        options.TimestampColumnName = "recorded_at";
        options.TypeColumnName = "event_type";
        options.PayloadColumnName = "event_payload";

        Console.WriteLine($"Event table: {options.TableName}, ID column: {options.CompositePrimaryKeyIdColumnName}");
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

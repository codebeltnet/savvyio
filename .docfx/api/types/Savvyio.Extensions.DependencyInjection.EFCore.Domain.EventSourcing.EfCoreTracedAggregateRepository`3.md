---
uid: Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing.EfCoreTracedAggregateRepository`3
example:
- *content
---
This example shows how `EfCoreTracedAggregateRepository<TEntity, TKey, TMarker>` can be resolved when event-sourced aggregates need both a marker and a marshaller.

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Savvyio;
using Savvyio.Domain.EventSourcing;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.Domain.EventSourcing;
using Savvyio.Extensions.DependencyInjection.EFCore;
using Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing;
using Savvyio.Extensions.EFCore.Domain.EventSourcing;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class MarkerTracedRepositoryExample
{
    public bool IsResolvedAsMarkedTracedRepository()
    {
        var services = new ServiceCollection();
        services.AddMarshaller<SimpleMarshaller>();
        services.AddEfCoreDataSource<TimelineMarker>(options =>
        {
            options.ContextConfigurator = builder => builder.EnableDetailedErrors();
            options.ModelConstructor = modelBuilder => modelBuilder.AddEventSourcing<OrderTimeline, Guid>();
        });
        services.AddEfCoreTracedAggregateRepository<OrderTimeline, Guid, TimelineMarker>();

        using var provider = services.BuildServiceProvider();
        var repository = provider.GetRequiredService<ITracedAggregateRepository<OrderTimeline, Guid, TimelineMarker>>();

        return repository is EfCoreTracedAggregateRepository<OrderTimeline, Guid, TimelineMarker>;
    }
}

public sealed class TimelineMarker
{
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

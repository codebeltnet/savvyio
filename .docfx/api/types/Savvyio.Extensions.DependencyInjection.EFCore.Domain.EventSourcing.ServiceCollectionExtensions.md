---
uid: Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing.ServiceCollectionExtensions
example:
- *content
---
Event-sourced aggregates backed by EF Core need a data source registered by `AddEfCoreAggregateDataSource` and a traced repository registered by `AddEfCoreTracedAggregateRepository`. The schema must be configured separately in `OnModelCreating` using `ModelBuilderExtensions.AddEventSourcing`. The example registers both services alongside the marshaller and resolves the traced repository interface to verify the full event-sourcing registration.

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

public sealed class TracedRegistrationExample
{
    public ServiceProvider BuildProvider()
    {
        var services = new ServiceCollection();
        services.AddMarshaller<SimpleMarshaller>();
        services.AddEfCoreDataSource(options =>
        {
            options.ContextConfigurator = builder => builder.EnableDetailedErrors();
            options.ModelConstructor = modelBuilder => modelBuilder.AddEventSourcing<OrderTimeline, Guid>();
        });
        services.AddEfCoreTracedAggregateRepository<OrderTimeline, Guid>();

        return services.BuildServiceProvider();
    }

    public bool HasExpectedRegistration(ServiceProvider provider)
    {
        var repository = provider.GetRequiredService<ITracedAggregateRepository<OrderTimeline, Guid>>();
        return repository is Savvyio.Extensions.EFCore.Domain.EventSourcing.EfCoreTracedAggregateRepository<OrderTimeline, Guid>;
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

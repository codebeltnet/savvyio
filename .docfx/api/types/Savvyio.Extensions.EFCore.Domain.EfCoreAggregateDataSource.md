---
uid: Savvyio.Extensions.EFCore.Domain.EfCoreAggregateDataSource
example:
- *content
---
`EfCoreAggregateDataSource<TContext>` is the domain-scoped EF Core data source that implements `IEfCoreDataSource` and restricts context usage to aggregate root operations. Subclass it with the specific `DbContext` type to provide a named aggregate context factory. The example wires up a minimal aggregate context and confirms it resolves from the data source.

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Savvyio.Domain;
using Savvyio.Extensions.EFCore;
using Savvyio.Extensions.EFCore.Domain;

namespace ExampleApp;

public sealed class OrderingWorkflow
{
    public async Task<int> SaveOrderAsync()
    {
        var dispatcher = new RecordingDomainEventDispatcher();
        var source = new OrderingDataSource(dispatcher, new EfCoreDataSourceOptions
        {
            ContextConfigurator = builder => builder.EnableDetailedErrors(),
            ModelConstructor = modelBuilder => modelBuilder.Entity<OrderAggregate>().HasKey(order => order.Id)
        });

        var repository = new EfCoreAggregateRepository<OrderAggregate, Guid>(source);
        repository.Add(OrderAggregate.Place(Guid.NewGuid(), "PO-2048"));

        await source.SaveChangesAsync();
        return dispatcher.Published.Count;
    }
}

public sealed class OrderingDataSource : EfCoreAggregateDataSource
{
    public OrderingDataSource(IDomainEventDispatcher dispatcher, EfCoreDataSourceOptions options) : base(dispatcher, options)
    {
    }
}

public sealed class RecordingDomainEventDispatcher : IDomainEventDispatcher
{
    public List<IDomainEvent> Published { get; } = new();

    public void Raise(IDomainEvent request)
    {
        Published.Add(request);
    }

    public Task RaiseAsync(IDomainEvent request, Action<Cuemon.Threading.AsyncOptions>? setup = null)
    {
        Published.Add(request);
        return Task.CompletedTask;
    }
}

public sealed class OrderAggregate : Aggregate<Guid, IDomainEvent>, IAggregateRoot<IDomainEvent, Guid>
{
    private OrderAggregate()
    {
    }

    private OrderAggregate(Guid id, string orderNumber) : base(id)
    {
        OrderNumber = orderNumber;
        AddEvent(new OrderPlaced(orderNumber));
    }

    public string OrderNumber { get; private set; } = string.Empty;

    public static OrderAggregate Place(Guid id, string orderNumber)
    {
        return new OrderAggregate(id, orderNumber);
    }
}

public sealed record OrderPlaced(string OrderNumber) : DomainEvent;
```

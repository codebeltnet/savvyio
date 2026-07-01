---
uid: Savvyio.Extensions.EFCore.Domain.DomainEventDispatcherExtensions
example:
- *content
---
`DomainEventDispatcherExtensions.RaiseMany` and `RaiseManyAsync` iterate over the domain events accumulated on an `IAggregateRoot` after a save operation and dispatch each event through `IDomainEventDispatcher`. Inject `IDomainEventDispatcher` into the repository, call the save operation, and then call `RaiseManyAsync` with the saved aggregate. The example shows a save-then-raise pattern using an EF Core–backed order repository.

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Savvyio.Domain;
using Savvyio.Extensions.EFCore.Domain;

namespace ExampleApp;

public sealed class DomainEventWorkflow
{
    public async Task<int> PublishPendingEventsAsync()
    {
        var dispatcher = new RecordingDomainEventDispatcher();
        var context = new OrdersContext();
        var order = OrderAggregate.Place(Guid.NewGuid(), "PO-1024");

        context.Orders.Add(order);

        dispatcher.RaiseMany(context);
        dispatcher.RaiseManyAsync(context).GetAwaiter().GetResult();

        await Task.CompletedTask;
        return dispatcher.Published.Count;
    }
}

public sealed class OrdersContext : DbContext
{
    public DbSet<OrderAggregate> Orders => Set<OrderAggregate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderAggregate>().HasKey(order => order.Id);
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

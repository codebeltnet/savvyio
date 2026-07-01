---
uid: Savvyio.Extensions.EFCore.Domain.EfCoreAggregateRepository`2
example:
- *content
---
`EfCoreAggregateRepository<TAggregateRoot, TKey, TContext>` provides EF Core persistence for aggregate roots with strict boundary enforcement. Subclass it by providing the aggregate root type and key type, then inject the aggregate data source. The example creates a concrete repository, calls `AddAsync`, and resolves the aggregate to verify the persistence workflow.

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Savvyio.Domain;
using Savvyio.Extensions.EFCore;
using Savvyio.Extensions.EFCore.Domain;

namespace ExampleApp;

public sealed class OrderingQueries
{
    public Task<IEnumerable<OrderAggregate>> LoadPriorityOrdersAsync()
    {
        var source = new OrderingDataSource(new EfCoreDataSourceOptions());
        var repository = new OrderingRepository(source);

        repository.Add(OrderAggregate.Place(Guid.NewGuid(), "PO-4096", true));
        return repository.FindPriorityOrdersAsync();
    }
}

public sealed class OrderingDataSource : EfCoreDataSource
{
    public OrderingDataSource(EfCoreDataSourceOptions options) : base(options)
    {
    }
}

public sealed class OrderingRepository : EfCoreAggregateRepository<OrderAggregate, Guid>
{
    public OrderingRepository(IEfCoreDataSource source) : base(source)
    {
    }

    public Task<IEnumerable<OrderAggregate>> FindPriorityOrdersAsync()
    {
        return FindAllAsync(order => order.IsPriority);
    }
}

public sealed class OrderAggregate : Aggregate<Guid, IDomainEvent>, IAggregateRoot<IDomainEvent, Guid>
{
    private OrderAggregate()
    {
    }

    private OrderAggregate(Guid id, string orderNumber, bool isPriority) : base(id)
    {
        OrderNumber = orderNumber;
        IsPriority = isPriority;
        AddEvent(new OrderPlaced(orderNumber));
    }

    public string OrderNumber { get; private set; } = string.Empty;

    public bool IsPriority { get; private set; }

    public static OrderAggregate Place(Guid id, string orderNumber, bool isPriority)
    {
        return new OrderAggregate(id, orderNumber, isPriority);
    }
}

public sealed record OrderPlaced(string OrderNumber) : DomainEvent;
```

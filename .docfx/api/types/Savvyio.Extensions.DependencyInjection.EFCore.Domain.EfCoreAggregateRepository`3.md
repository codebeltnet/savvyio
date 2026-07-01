---
uid: Savvyio.Extensions.DependencyInjection.EFCore.Domain.EfCoreAggregateRepository`3
example:
- *content
---
The DI-registered `EfCoreAggregateRepository<TAggregateRoot, TKey, TContext>` resolves as `IAggregateRepository<TAggregateRoot, TKey>` when registered via `AddEfCoreAggregateRepository`. The aggregate data source registered by `AddEfCoreAggregateDataSource` provides the context. The example registers both services and resolves the repository interface.

```csharp
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.DependencyInjection.EFCore;
using Savvyio.Extensions.DependencyInjection.EFCore.Domain;

namespace ExampleApp;

public sealed class MarkerAggregateRepositoryExample
{
    public bool IsResolvedAsMarkedAggregateRepository()
    {
        var services = new ServiceCollection();
        services.AddEfCoreDataSource<OrderingMarker>(options =>
        {
            options.ContextConfigurator = builder => builder.EnableDetailedErrors();
            options.ModelConstructor = modelBuilder => modelBuilder.Entity<OrderAggregate>().HasKey(order => order.Id);
        });
        services.AddEfCoreAggregateRepository<OrderAggregate, Guid, OrderingMarker>();

        using var provider = services.BuildServiceProvider();
        var repository = provider.GetRequiredService<IAggregateRepository<OrderAggregate, Guid, OrderingMarker>>();

        return repository is EfCoreAggregateRepository<OrderAggregate, Guid, OrderingMarker>;
    }
}

public sealed class OrderingMarker
{
}

public sealed class OrderAggregate : Aggregate<Guid, IDomainEvent>, IAggregateRoot<IDomainEvent, Guid>
{
    public OrderAggregate(Guid id) : base(id)
    {
    }
}
```

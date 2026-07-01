---
uid: Savvyio.Extensions.DependencyInjection.EFCore.Domain.EfCoreAggregateDataSource`1
example:
- *content
---
This example shows how `EfCoreAggregateDataSource<TMarker>` can be resolved from DI when an aggregate store needs a marker-specific pipeline.

```csharp
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection.EFCore;
using Savvyio.Extensions.DependencyInjection.EFCore.Domain;

namespace ExampleApp;

public sealed class MarkerAggregateDataSourceExample
{
    public bool IsResolvedAsMarkedAggregateSource()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IDomainEventDispatcher, RecordingDomainEventDispatcher>();
        services.AddEfCoreAggregateDataSource<OrderingMarker>(options =>
        {
            options.ContextConfigurator = builder => builder.EnableDetailedErrors();
            options.ModelConstructor = modelBuilder => modelBuilder.Entity<OrderAggregate>().HasKey(order => order.Id);
        });

        using var provider = services.BuildServiceProvider();
        var source = provider.GetRequiredService<IEfCoreDataSource<OrderingMarker>>();

        return source is EfCoreAggregateDataSource<OrderingMarker>;
    }
}

public sealed class OrderingMarker
{
}

public sealed class RecordingDomainEventDispatcher : IDomainEventDispatcher
{
    public void Raise(IDomainEvent request)
    {
    }

    public Task RaiseAsync(IDomainEvent request, Action<Cuemon.Threading.AsyncOptions>? setup = null)
    {
        return Task.CompletedTask;
    }
}

public sealed class OrderAggregate : Aggregate<Guid, IDomainEvent>, IAggregateRoot<IDomainEvent, Guid>
{
    public OrderAggregate(Guid id) : base(id)
    {
    }
}
```

---
uid: Savvyio.Extensions.DependencyInjection.EFCore.Domain.ServiceCollectionExtensions
example:
- *content
---
Registering domain aggregates backed by EF Core requires two registrations: `AddEfCoreAggregateDataSource` to bind the aggregate context as `IEfCoreDataSource`, and `AddEfCoreAggregateRepository` to add the repository on top. The aggregate data source sets the boundary for which context the repository uses; it must be registered first. The example registers both services and resolves the repository interface to confirm the aggregate wiring.

```csharp
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.DependencyInjection.EFCore;
using Savvyio.Extensions.DependencyInjection.EFCore.Domain;

namespace ExampleApp;

public sealed class AggregateRegistrationExample
{
    public ServiceProvider BuildProvider()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IDomainEventDispatcher, RecordingDomainEventDispatcher>();
        services.AddEfCoreAggregateDataSource(options =>
        {
            options.ContextConfigurator = builder => builder.EnableDetailedErrors();
            options.ModelConstructor = modelBuilder => modelBuilder.Entity<OrderAggregate>().HasKey(order => order.Id);
        });
        services.AddEfCoreAggregateRepository<OrderAggregate, Guid>();

        return services.BuildServiceProvider();
    }

    public bool HasExpectedRegistrations(ServiceProvider provider)
    {
        var repository = provider.GetRequiredService<IAggregateRepository<OrderAggregate, Guid>>();

        return repository is Savvyio.Extensions.EFCore.Domain.EfCoreAggregateRepository<OrderAggregate, Guid>;
    }
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

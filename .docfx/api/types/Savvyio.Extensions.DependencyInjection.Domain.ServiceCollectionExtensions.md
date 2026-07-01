---
uid: Savvyio.Extensions.DependencyInjection.Domain.ServiceCollectionExtensions
example:
- *content
---
Registering DDD aggregate repositories in Savvy I/O involves `AddAggregateRepository` for aggregate persistence, `AddRepository` for read-model repositories, and `AddUnitOfWork` for coordinating saves. Each call takes the service interface, entity type, and key type as type arguments. The example registers all three patterns alongside a shared data source and resolves each service to verify the full domain persistence wiring.

```csharp
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon.Threading;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection;
using Savvyio;
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection.Domain;

namespace ExampleApp;

public static class DomainRegistration
{
    public static IServiceCollection AddOrderPersistence(this IServiceCollection services)
    {
        services.AddDataSource<OrderDataSource>();
        services.AddUnitOfWork<OrderDataSource>();
        services.AddAggregateRepository<OrderRepository, OrderAggregate, Guid>();
        services.AddRepository<OrderRepository, OrderAggregate, Guid>();
        return services;
    }
}

public sealed class OrderDataSource : IDataSource, IUnitOfWork
{
    public Task SaveChangesAsync(Action<AsyncOptions> setup = null)
    {
        return Task.CompletedTask;
    }
}

public sealed class OrderAggregate : AggregateRoot<Guid>
{
    public OrderAggregate(Guid id, string customerId) : base(id)
    {
        CustomerId = customerId;
    }

    public string CustomerId { get; private set; }
}

public sealed class OrderRepository : IAggregateRepository<OrderAggregate, Guid>
{
    public OrderRepository(Savvyio.IDataSource dataSource)
    {
    }

    public void Add(OrderAggregate entity)
    {
    }

    public void AddRange(IEnumerable<OrderAggregate> entities)
    {
    }

    public Task<IEnumerable<OrderAggregate>> FindAllAsync(Expression<Func<OrderAggregate, bool>> predicate = null, Action<AsyncOptions> setup = null)
    {
        return Task.FromResult<IEnumerable<OrderAggregate>>(Array.Empty<OrderAggregate>());
    }

    public Task<OrderAggregate> FindAsync(Expression<Func<OrderAggregate, bool>> predicate, Action<AsyncOptions> setup = null)
    {
        return Task.FromResult(new OrderAggregate(Guid.NewGuid(), "ALFKI"));
    }

    public Task<OrderAggregate> GetByIdAsync(Guid id, Action<AsyncOptions> setup = null)
    {
        return Task.FromResult(new OrderAggregate(id, "ALFKI"));
    }

    public void Remove(OrderAggregate entity)
    {
    }

    public void RemoveRange(IEnumerable<OrderAggregate> entities)
    {
    }
}
```

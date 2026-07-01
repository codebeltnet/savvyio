---
uid: Savvyio.Extensions.DependencyInjection.Domain.EventSourcing.ServiceCollectionExtensions
example:
- *content
---
`AddTracedAggregateRepository` binds `ITracedAggregateRepository<TAggregateRoot, TKey>` to its implementation. Call it alongside `AddDataSource` and `AddUnitOfWork` to complete the event-sourcing persistence registration.

```csharp
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon.Threading;
using Microsoft.Extensions.DependencyInjection;
using Savvyio;
using Savvyio.Domain;
using Savvyio.Domain.EventSourcing;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.DependencyInjection.Domain.EventSourcing;

namespace ExampleApp;

public static class EventSourcingRegistration
{
    public static IServiceCollection Configure(IServiceCollection services)
    {
        services.AddDataSource<EventStoreSource>();
        services.AddUnitOfWork<EventStoreSource>();
        services.AddTracedAggregateRepository<OrderHistoryRepository, OrderHistory, Guid>();
        return services;
    }
}

public sealed class EventStoreSource : Savvyio.IDataSource, IUnitOfWork
{
    public Task SaveChangesAsync(Action<AsyncOptions> setup = null) => Task.CompletedTask;
}

public sealed class OrderHistory : ITracedAggregateRoot<Guid>
{
    public long Version => 0;
    public IReadOnlyList<ITracedDomainEvent> Events => Array.Empty<ITracedDomainEvent>();
    public void RemoveAllEvents() { }
    public IMetadataDictionary Metadata { get; } = new MetadataDictionary();
    public Guid Id { get; } = Guid.NewGuid();
}

public sealed class OrderHistoryRepository : ITracedAggregateRepository<OrderHistory, Guid>
{
    public OrderHistoryRepository(Savvyio.IDataSource source) { }
    public void Add(OrderHistory entity) { }
    public void AddRange(IEnumerable<OrderHistory> entities) { }
    public void Remove(OrderHistory entity) { }
    public void RemoveRange(IEnumerable<OrderHistory> entities) { }
    public Task<OrderHistory> FindAsync(Expression<Func<OrderHistory, bool>> predicate, Action<AsyncOptions> setup = null) => Task.FromResult(new OrderHistory());
    public Task<IEnumerable<OrderHistory>> FindAllAsync(Expression<Func<OrderHistory, bool>> predicate = null, Action<AsyncOptions> setup = null) => Task.FromResult<IEnumerable<OrderHistory>>(Array.Empty<OrderHistory>());
    public Task<OrderHistory> GetByIdAsync(Guid id, Action<AsyncOptions> setup = null) => Task.FromResult(new OrderHistory());
}
```





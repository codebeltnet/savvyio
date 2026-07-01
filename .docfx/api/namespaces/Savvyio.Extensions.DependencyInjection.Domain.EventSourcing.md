---
uid: Savvyio.Extensions.DependencyInjection.Domain.EventSourcing
summary: *content
---
Event-sourced aggregates require a specialized repository that stores and replays traced domain events rather than current state. The `Savvyio.Extensions.DependencyInjection.Domain.EventSourcing` namespace provides `AddTracedAggregateRepository<TService, TEntity, TKey>` and the `ITracedAggregateRepository` marker interface.

Start with `AddTracedAggregateRepository<TService, TEntity, TKey>` to bind an `ITracedAggregateRepository` to its implementation. For the EF Core–backed implementation, use `Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddTracedAggregateRepository<TService, TEntity, TKey>`|

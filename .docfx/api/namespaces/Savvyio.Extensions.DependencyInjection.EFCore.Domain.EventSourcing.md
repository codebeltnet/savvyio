---
uid: Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing
summary: *content
---
Event sourcing with EF Core requires a dedicated repository that writes traced domain events. The `Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing` namespace provides `AddEfCoreTracedAggregateRepository<TEntity, TKey>` and `AddEfCoreTracedAggregateRepository<TEntity, TKey, TMarker>` to register this repository.

Start with `AddEfCoreTracedAggregateRepository<TEntity, TKey>` to bind `ITracedAggregateRepository` to `EfCoreTracedAggregateRepository`. Choose this namespace when your domain uses event sourcing and stores aggregate history as a sequence of immutable traced events in a relational database; for standard non-event-sourced aggregates with EF Core, use `Savvyio.Extensions.DependencyInjection.EFCore.Domain` instead. Pair with `Savvyio.Extensions.EFCore.Domain.EventSourcing` which provides the `ModelBuilder` extension to create the event-store schema in `OnModelCreating`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddEfCoreTracedAggregateRepository<TEntity, TKey>`, `AddEfCoreTracedAggregateRepository<TEntity, TKey, TMarker>`|

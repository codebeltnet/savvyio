---
uid: Savvyio.Extensions.EFCore.Domain
summary: *content
---
Aggregate roots require their own persistence boundary in DDD. The `Savvyio.Extensions.EFCore.Domain` namespace provides `EfCoreAggregateRepository` and `EfCoreAggregateDataSource` for EF Core–backed aggregate persistence, and `DomainEventDispatcherExtensions` to dispatch accumulated domain events after the aggregate is saved.

Start with `EfCoreAggregateRepository<TAggregateRoot, TKey, TContext>` as the base class for aggregate root repositories. Extend it and inject `IDomainEventDispatcher`, then call `RaiseManyAsync<T>` after saving to dispatch accumulated domain events. Register with `Savvyio.Extensions.DependencyInjection.EFCore.Domain`. For event-sourced aggregates, use `Savvyio.Extensions.EFCore.Domain.EventSourcing`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IDomainEventDispatcher|⬇️|`RaiseMany`, `RaiseManyAsync`|

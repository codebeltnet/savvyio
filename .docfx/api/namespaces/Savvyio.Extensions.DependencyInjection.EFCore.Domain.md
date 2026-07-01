---
uid: Savvyio.Extensions.DependencyInjection.EFCore.Domain
summary: *content
---
Aggregate roots require their own context boundary. The `Savvyio.Extensions.DependencyInjection.EFCore.Domain` namespace provides `AddEfCoreAggregateDataSource<TMarker>` and `AddEfCoreAggregateRepository<TEntity, TKey>` to register EF Core persistence for aggregate roots.

Start with `AddEfCoreAggregateDataSource<TMarker>` to configure the `DbContext` as a domain data source. Then add `AddEfCoreAggregateRepository<TEntity, TKey>` for each aggregate root type. For event-sourced aggregates, use `Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddEfCoreAggregateDataSource`, `AddEfCoreAggregateDataSource<TMarker>`, `AddEfCoreAggregateRepository<TEntity, TKey>`, `AddEfCoreAggregateRepository<TEntity, TKey, TMarker>`|

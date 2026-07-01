---
uid: Savvyio.Extensions.DependencyInjection.EFCore
summary: *content
---
One DI call to configure both the `DbContext` and the Savvy I/O data layer is the goal of this namespace. `AddEfCoreDataSource<TMarker>`, `AddEfCoreDataStore<T>`, and `AddEfCoreRepository<TEntity, TKey>` wire up Entity Framework Core persistence without boilerplate.

Start with `AddEfCoreDataSource<TMarker>` to register the `DbContext` and the `IEfCoreDataSource`. Then add `AddEfCoreRepository<TEntity, TKey>` or `AddEfCoreDataStore<T>` for each repository or data store. For domain aggregates with explicit boundaries, prefer `Savvyio.Extensions.DependencyInjection.EFCore.Domain`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddEfCoreDataSource`, `AddEfCoreDataSource<TMarker>`, `AddEfCoreDataStore<T>`, `AddEfCoreDataStore<T, TMarker>`, `AddEfCoreRepository<TEntity, TKey>`, `AddEfCoreRepository<TEntity, TKey, TMarker>`|

---
uid: Savvyio.Extensions.DependencyInjection.Domain
summary: *content
---
Registering DDD aggregate repositories in the DI container is done through this namespace. It provides `AddAggregateRepository<TService, TEntity, TKey>`, `AddRepository<TService, TEntity, TKey>`, and `AddUnitOfWork<TService>` for binding aggregate and read-model repository contracts to their implementations.

Start with `AddAggregateRepository<TService, TEntity, TKey>` to register the primary aggregate repository. Use `AddUnitOfWork<TService>` when your domain layer requires a unit-of-work coordinator. For event-sourced aggregates, add `Savvyio.Extensions.DependencyInjection.Domain.EventSourcing`. For EF Core–backed implementations, prefer `Savvyio.Extensions.DependencyInjection.EFCore.Domain`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddAggregateRepository<TService, TEntity, TKey>`, `AddRepository<TService, TEntity, TKey>`, `AddUnitOfWork<TService>`|

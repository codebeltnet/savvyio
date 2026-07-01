---
uid: Savvyio.Extensions.EFCore
summary: *content
---
Use the `Savvyio.Extensions.EFCore` namespace for the core Entity Framework Core integration types. It provides `EfCoreDbContext`, `EfCoreDataSource`, `EfCoreDataStore`, and `EfCoreRepository` — the building blocks for infrastructure-layer persistence that connects EF Core to the Savvy I/O data access abstractions in `Savvyio.Data`.

Start with `EfCoreDataSource<TContext>` as the base class when you want a typed `DbContext` factory that implements `IEfCoreDataSource`. Extend `EfCoreDataStore<T, TContext>` for general data store operations, or `EfCoreRepository<TAggregateRoot, TKey, TContext>` for aggregate root–scoped repositories. Register these with `Savvyio.Extensions.DependencyInjection.EFCore`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

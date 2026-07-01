---
uid: Savvyio.Extensions.DependencyInjection.Data
summary: *content
---
Registering data stores without tying to a technology keeps the domain model portable. The `Savvyio.Extensions.DependencyInjection.Data` namespace provides `AddDataStore<TService, T>` and `AddDataStore<TService, T, TOptions>` for this purpose.

Start with `AddDataStore<TService, T>` to bind an `IDataStore` interface to its implementation. Choose this namespace when your domain layer depends only on the abstract `IDataStore` interface family and you want the concrete implementation resolved by DI without referencing a specific ORM or data-access library. For technology-specific registrations that also configure a connection or context, see `Savvyio.Extensions.DependencyInjection.EFCore`, `Savvyio.Extensions.DependencyInjection.Dapper`, or `Savvyio.Extensions.DependencyInjection.DapperExtensions`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddDataStore<TService, T>`, `AddDataStore<TService, T, TOptions>`|

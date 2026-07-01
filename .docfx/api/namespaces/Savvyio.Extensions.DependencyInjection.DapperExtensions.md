---
uid: Savvyio.Extensions.DependencyInjection.DapperExtensions
summary: *content
---
DapperExtensions adds automatic CRUD on top of Dapper. The `Savvyio.Extensions.DependencyInjection.DapperExtensions` namespace provides `AddDapperExtensionsDataStore<T>` and `AddDapperExtensionsDataStore<T, TMarker>` to register the `DapperExtensionsDataStore` implementation from `Savvyio.Extensions.DapperExtensions`.

Start with `AddDapperExtensionsDataStore<T>` to bind a data store interface to its DapperExtensions implementation. Choose this namespace when you want automatic CRUD without writing SQL statements — DapperExtensions infers INSERT/UPDATE/DELETE/SELECT from class mapping conventions. For handwritten SQL, use `Savvyio.Extensions.DependencyInjection.Dapper` instead. Combine with `Savvyio.Extensions.DependencyInjection.Dapper` for the shared connection factory.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddDapperExtensionsDataStore<T>`, `AddDapperExtensionsDataStore<T, TMarker>`|

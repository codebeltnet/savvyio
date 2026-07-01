---
uid: Savvyio.Extensions.DependencyInjection.Dapper
summary: *content
---
Dapper persistence needs both a connection factory and a data store. The `Savvyio.Extensions.DependencyInjection.Dapper` namespace provides `AddDapperDataSource<TMarker>` for the connection factory and `AddDapperDataStore<TService, T>` for each data store, combining both concerns in one namespace.

Start with `AddDapperDataSource<TMarker>` to configure the connection and register `IDapperDataSource`. Then add `AddDapperDataStore<TService, T>` for each data store your application needs. Choose this namespace when you want lightweight, handwritten SQL via Dapper without an ORM; for convention-based automatic CRUD instead, prefer `Savvyio.Extensions.DependencyInjection.DapperExtensions`, and for EF Core, use `Savvyio.Extensions.DependencyInjection.EFCore`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddDapperDataSource`, `AddDapperDataSource<TMarker>`, `AddDapperDataSource<TService>`, `AddDapperDataStore<TService, T>`|

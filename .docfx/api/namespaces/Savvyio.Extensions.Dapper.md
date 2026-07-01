---
uid: Savvyio.Extensions.Dapper
summary: *content
---
Use the `Savvyio.Extensions.Dapper` namespace to access data using Dapper — a lightweight micro-ORM that executes raw SQL and maps results to your domain objects. It provides `DapperDataSource` as the connection factory and `DapperDataStore` as the base class for Dapper-backed read and write data stores.

Start with `DapperDataSource<TContext>` to wrap a database connection factory that implements `IDapperDataSource`. Extend `DapperDataStore<T, TContext>` to write your repository logic using Dapper's `Execute`, `Query`, and `QueryAsync` methods. Configure query options through `DapperQueryOptions`. Register with `Savvyio.Extensions.DependencyInjection.Dapper`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

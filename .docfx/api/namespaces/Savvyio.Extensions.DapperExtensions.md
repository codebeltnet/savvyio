---
uid: Savvyio.Extensions.DapperExtensions
summary: *content
---
DapperExtensions adds automatic CRUD mapping on top of Dapper. The `Savvyio.Extensions.DapperExtensions` namespace provides `DapperExtensionsDataStore` as the base class and `DapperExtensionsQueryOptions` for configuring query execution.

Start with `DapperExtensionsDataStore<T, TContext>` when you want auto-mapped CRUD without writing SQL for each operation. Configure query behavior through `DapperExtensionsQueryOptions`. Register with `Savvyio.Extensions.DependencyInjection.DapperExtensions`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

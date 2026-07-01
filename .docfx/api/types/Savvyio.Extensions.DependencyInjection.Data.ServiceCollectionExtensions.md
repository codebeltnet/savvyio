---
uid: Savvyio.Extensions.DependencyInjection.Data.ServiceCollectionExtensions
example:
- *content
---
Register a custom data source and persistent data store with `AddDataStore` for application read and write models.

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.Threading;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection;
using Savvyio;
using Savvyio.Data;
using Savvyio.Extensions.DependencyInjection.Data;

namespace ExampleApp;

public static class DataStoreRegistration
{
    public static IServiceCollection AddOrdersDataStore(this IServiceCollection services)
    {
        services.AddDataSource<InMemoryDataSource>();
        services.AddDataStore<OrderDataStore, OrderDocument>();
        return services;
    }
}

public sealed class InMemoryDataSource : Savvyio.IDataSource
{
}

public sealed class OrderDocument
{
    public string Id { get; init; } = string.Empty;
}

public sealed class OrderDataStore : IPersistentDataStore<OrderDocument, AsyncOptions>
{
    public OrderDataStore(Savvyio.IDataSource dataSource)
    {
    }

    public Task CreateAsync(OrderDocument dto, Action<AsyncOptions> setup = null)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(OrderDocument dto, Action<AsyncOptions> setup = null)
    {
        return Task.CompletedTask;
    }

    public Task<IEnumerable<OrderDocument>> FindAllAsync(Action<AsyncOptions> setup = null)
    {
        return Task.FromResult<IEnumerable<OrderDocument>>(Array.Empty<OrderDocument>());
    }

    public Task<OrderDocument> FindAsync(Action<AsyncOptions> setup = null)
    {
        return Task.FromResult(new OrderDocument());
    }

    public Task<OrderDocument> GetByIdAsync(object id, Action<AsyncOptions> setup = null)
    {
        return Task.FromResult(new OrderDocument { Id = id?.ToString() ?? string.Empty });
    }

    public Task UpdateAsync(OrderDocument dto, Action<AsyncOptions> setup = null)
    {
        return Task.CompletedTask;
    }
}
```

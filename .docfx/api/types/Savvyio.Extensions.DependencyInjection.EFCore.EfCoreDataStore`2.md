---
uid: Savvyio.Extensions.DependencyInjection.EFCore.EfCoreDataStore`2
example:
- *content
---
The DI-registered `EfCoreDataStore<T, TContext>` resolves as `IPersistentDataStore<T, EfCoreQueryOptions<T>>` when registered via `AddEfCoreDataStore`. The data source registered by `AddEfCoreDataSource` provides the context; the concrete store type adds entity-specific query logic. The example resolves the store from the provider and confirms the registered concrete type.

```csharp
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Savvyio;
using Savvyio.Data;
using Savvyio.Extensions.DependencyInjection.Data;
using Savvyio.Extensions.DependencyInjection.EFCore;
using Savvyio.Extensions.EFCore;

namespace ExampleApp;

public sealed class MarkerBasedDataStoreExample
{
    public bool IsResolvedAsMarkedStore()
    {
        var services = new ServiceCollection();
        services.AddEfCoreDataSource<CatalogMarker>(options =>
        {
            options.ContextConfigurator = builder => builder.EnableDetailedErrors();
            options.ModelConstructor = modelBuilder => modelBuilder.Entity<Product>().HasKey(product => product.Id);
        });
        services.AddEfCoreDataStore<Product, CatalogMarker>();

        using var provider = services.BuildServiceProvider();
        var store = provider.GetRequiredService<IPersistentDataStore<Product, EfCoreQueryOptions<Product>, CatalogMarker>>();

        return store is EfCoreDataStore<Product, CatalogMarker>;
    }
}

public sealed class CatalogMarker
{
}

public sealed class Product : IIdentity<Guid>
{
    public Product(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; }

    public string Name { get; private set; } = string.Empty;
}
```

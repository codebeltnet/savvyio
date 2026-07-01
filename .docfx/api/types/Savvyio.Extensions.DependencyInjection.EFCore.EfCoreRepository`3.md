---
uid: Savvyio.Extensions.DependencyInjection.EFCore.EfCoreRepository`3
example:
- *content
---
The DI-registered `EfCoreRepository<TEntity, TKey, TContext>` resolves as `IRepository<TEntity, TKey>` when registered via `AddEfCoreRepository`. The data source registered by `AddEfCoreDataSource` provides the context. The example registers and resolves the repository to verify the complete EF Core repository wiring.

```csharp
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Savvyio;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.DependencyInjection.EFCore;

namespace ExampleApp;

public sealed class MarkerRepositoryExample
{
    public bool IsResolvedAsMarkedRepository()
    {
        var services = new ServiceCollection();
        services.AddEfCoreDataSource<CatalogMarker>(options =>
        {
            options.ContextConfigurator = builder => builder.EnableDetailedErrors();
            options.ModelConstructor = modelBuilder => modelBuilder.Entity<Product>().HasKey(product => product.Id);
        });
        services.AddEfCoreRepository<Product, Guid, CatalogMarker>();

        using var provider = services.BuildServiceProvider();
        var repository = provider.GetRequiredService<IPersistentRepository<Product, Guid, CatalogMarker>>();

        return repository is EfCoreRepository<Product, Guid, CatalogMarker>;
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

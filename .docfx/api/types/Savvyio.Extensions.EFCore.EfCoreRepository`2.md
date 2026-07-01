---
uid: Savvyio.Extensions.EFCore.EfCoreRepository`2
example:
- *content
---
`EfCoreRepository<TEntity, TKey, TContext>` is the EF Core aggregate repository base class for types that extend `IAggregateRoot<TKey>`. Subclass it, supply the aggregate type and key type, and inject the aggregate data source. The example creates a concrete repository and exercises a simple add-and-find workflow using an in-memory provider.

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Savvyio;
using Savvyio.Extensions.EFCore;

namespace ExampleApp;

public sealed class CatalogWorkflow
{
    public Task<IEnumerable<Product>> LoadFeaturedProductsAsync()
    {
        var source = new CatalogDataSource(new EfCoreDataSourceOptions
        {
            ContextConfigurator = builder => builder.EnableDetailedErrors(),
            ModelConstructor = modelBuilder => modelBuilder.Entity<Product>().HasKey(product => product.Id)
        });

        var repository = new CatalogRepository(source);
        repository.Add(new Product(Guid.NewGuid(), "Starter kit", true));
        return repository.FindFeaturedAsync();
    }
}

public sealed class CatalogDataSource : EfCoreDataSource
{
    public CatalogDataSource(EfCoreDataSourceOptions options) : base(options)
    {
    }
}

public sealed class CatalogRepository : EfCoreRepository<Product, Guid>
{
    public CatalogRepository(IEfCoreDataSource source) : base(source)
    {
    }

    public Task<IEnumerable<Product>> FindFeaturedAsync()
    {
        return FindAllAsync(product => product.IsFeatured);
    }
}

public sealed class Product : IIdentity<Guid>
{
    public Product(Guid id, string name, bool isFeatured)
    {
        Id = id;
        Name = name;
        IsFeatured = isFeatured;
    }

    public Guid Id { get; }

    public string Name { get; private set; } = string.Empty;

    public bool IsFeatured { get; private set; }
}
```

---
uid: Savvyio.Extensions.EFCore.EfCoreDataStore`1
example:
- *content
---
`EfCoreDataStore<T, TContext>` provides EF Core–backed create, read, update, delete, and search operations. Subclass it with the entity type and context type, then inject the matching `IEfCoreDataSource` as the constructor argument. The example creates a concrete order data store and verifies it can be instantiated with the data source.

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Savvyio;
using Savvyio.Extensions.EFCore;

namespace ExampleApp;

public sealed class CatalogQueries
{
    public Task<IEnumerable<Product>> LoadFeaturedProductsAsync()
    {
        var source = new CatalogDataSource(new EfCoreDataSourceOptions
        {
            ContextConfigurator = builder => builder.EnableDetailedErrors(),
            ModelConstructor = modelBuilder => modelBuilder.Entity<Product>().HasKey(product => product.Id)
        });

        var store = new CatalogDataStore(source);
        return store.FindFeaturedAsync();
    }
}

public sealed class CatalogDataSource : EfCoreDataSource
{
    public CatalogDataSource(EfCoreDataSourceOptions options) : base(options)
    {
    }
}

public sealed class CatalogDataStore : EfCoreDataStore<Product>
{
    public CatalogDataStore(IEfCoreDataSource source) : base(source)
    {
    }

    public Task<IEnumerable<Product>> FindFeaturedAsync()
    {
        return FindAllAsync(options => options.Predicate = product => product.IsFeatured);
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

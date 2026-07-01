---
uid: Savvyio.Extensions.EFCore.EfCoreQueryOptions`1
example:
- *content
---
`EfCoreQueryOptions<T>` carries the EF Core query predicate and ordering settings used by `EfCoreDataStore<T, TContext>` read operations. Configure the `Predicate`, `Ordering`, and `MaxRows` properties to control the result set. The example sets up a filtered query for pending orders and verifies the options are applied.

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Savvyio;
using Savvyio.Extensions.EFCore;

namespace ExampleApp;

public sealed class ProductFiltering
{
    public Task<IEnumerable<Product>> LoadActiveProductsAsync()
    {
        var query = new EfCoreQueryOptions<Product>
        {
            Predicate = product => product.IsActive
        };

        var source = new CatalogDataSource(new EfCoreDataSourceOptions
        {
            ContextConfigurator = builder => builder.EnableDetailedErrors(),
            ModelConstructor = modelBuilder => modelBuilder.Entity<Product>().HasKey(product => product.Id)
        });

        var store = new EfCoreDataStore<Product>(source);
        return store.FindAllAsync(options => options.Predicate = query.Predicate);
    }
}

public sealed class CatalogDataSource : EfCoreDataSource
{
    public CatalogDataSource(EfCoreDataSourceOptions options) : base(options)
    {
    }
}

public sealed class Product : IIdentity<Guid>
{
    public Product(Guid id, string name, bool isActive)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
    }

    public Guid Id { get; }

    public string Name { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }
}
```

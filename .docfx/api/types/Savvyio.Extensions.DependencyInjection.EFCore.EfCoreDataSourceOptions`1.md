---
uid: Savvyio.Extensions.DependencyInjection.EFCore.EfCoreDataSourceOptions`1
example:
- *content
---
`EfCoreDataSourceOptions<TContext>` carries the `ContextConfigurator` and service lifetime values used by `AddEfCoreDataSource` to configure the `DbContextOptionsBuilder`. Set `ContextConfigurator` to a lambda that calls `UseInMemoryDatabase` or `UseSqlServer` on the builder. The example configures an in-memory database and demonstrates how the options type flows into the DI registration.

```csharp
using System;
using Microsoft.EntityFrameworkCore;
using Savvyio;
using Savvyio.Extensions.DependencyInjection.EFCore;

namespace ExampleApp;

public sealed class MarkerOptionsExample
{
    public EfCoreDbContext<CatalogMarker> CreateContext()
    {
        var options = new EfCoreDataSourceOptions<CatalogMarker>
        {
            ContextConfigurator = builder => builder.EnableSensitiveDataLogging(),
            ModelConstructor = modelBuilder => modelBuilder.Entity<Product>().HasKey(product => product.Id)
        };

        return new EfCoreDbContext<CatalogMarker>(options);
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

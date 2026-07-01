---
uid: Savvyio.Extensions.EFCore.EfCoreDataSourceOptions
example:
- *content
---
This example shows how `EfCoreDataSourceOptions` centralizes model and context configuration for an EF Core-backed source.

```csharp
using System;
using Microsoft.EntityFrameworkCore;
using Savvyio;
using Savvyio.Extensions.EFCore;

namespace ExampleApp;

public sealed class CatalogConfiguration
{
    public EfCoreDataSource CreateSource()
    {
        var options = new EfCoreDataSourceOptions
        {
            ContextConfigurator = builder => builder.EnableSensitiveDataLogging(),
            ModelConstructor = modelBuilder =>
            {
                modelBuilder.Entity<Product>().HasKey(product => product.Id);
                modelBuilder.Entity<Product>().Property(product => product.Name).HasMaxLength(128);
            }
        };

        return new EfCoreDataSource(options);
    }
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

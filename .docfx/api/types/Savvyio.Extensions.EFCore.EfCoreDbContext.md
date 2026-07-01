---
uid: Savvyio.Extensions.EFCore.EfCoreDbContext
example:
- *content
---
`EfCoreDbContext` is the Savvy I/O base context that accepts `EfCoreDataSourceOptions` for connection management and configuration. Subclass it to add `DbSet<T>` properties for your domain entities and override `OnModelCreating` for fluent EF Core configuration. The example defines a minimal order context using an in-memory provider and verifies the context can be created and disposed.

```csharp
using System;
using Microsoft.EntityFrameworkCore;
using Savvyio;
using Savvyio.Extensions.EFCore;

namespace ExampleApp;

public sealed class CatalogContextFactory
{
    public CatalogDbContext Create()
    {
        var options = new EfCoreDataSourceOptions
        {
            ContextConfigurator = builder => builder.EnableDetailedErrors(),
            ModelConstructor = modelBuilder => modelBuilder.Entity<Product>().HasKey(product => product.Id)
        };

        return new CatalogDbContext(options);
    }
}

public sealed class CatalogDbContext : EfCoreDbContext
{
    public CatalogDbContext(EfCoreDataSourceOptions options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().HasKey(product => product.Id);
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

---
uid: Savvyio.Extensions.EFCore.EfCoreDataSource
example:
- *content
---
This example shows how a custom `EfCoreDataSource` can wrap a dedicated `DbContext` for a catalog workflow.

```csharp
using System;
using Microsoft.EntityFrameworkCore;
using Savvyio;
using Savvyio.Extensions.EFCore;

namespace ExampleApp;

public sealed class CatalogWorkflow
{
    public CatalogDataSource CreateDataSource()
    {
        var options = new EfCoreDataSourceOptions
        {
            ContextConfigurator = builder => builder.EnableDetailedErrors(),
            ModelConstructor = modelBuilder => modelBuilder.Entity<Product>().HasKey(product => product.Id)
        };

        return new CatalogDataSource(new CatalogDbContext(options));
    }
}

public sealed class CatalogDataSource : EfCoreDataSource
{
    public CatalogDataSource(CatalogDbContext dbContext) : base(dbContext)
    {
    }

    public DbSet<Product> Products => Set<Product>();
}

public sealed class CatalogDbContext : DbContext
{
    private readonly EfCoreDataSourceOptions _options;

    public CatalogDbContext(EfCoreDataSourceOptions options)
    {
        _options = options;
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _options.ContextConfigurator?.Invoke(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _options.ModelConstructor?.Invoke(modelBuilder);
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

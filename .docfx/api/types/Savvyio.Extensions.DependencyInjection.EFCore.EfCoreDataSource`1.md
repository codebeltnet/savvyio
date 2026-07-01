---
uid: Savvyio.Extensions.DependencyInjection.EFCore.EfCoreDataSource`1
example:
- *content
---
The DI-registered `EfCoreDataSource<TContext>` is the concrete `IEfCoreDataSource` bound by `AddEfCoreDataSource`. Resolve it as `IEfCoreDataSource` to access the `DbContext` factory or pass it to EF Core repositories. The example registers the data source with an in-memory provider and resolves it from the service provider.

```csharp
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Savvyio;
using Savvyio.Extensions.DependencyInjection.EFCore;

namespace ExampleApp;

public sealed class MarkerBasedDataSourceExample
{
    public bool IsResolvedAsMarkedSource()
    {
        var services = new ServiceCollection();
        services.AddEfCoreDataSource<CatalogMarker>(options =>
        {
            options.ContextConfigurator = builder => builder.EnableDetailedErrors();
            options.ModelConstructor = modelBuilder => modelBuilder.Entity<Product>().HasKey(product => product.Id);
        });

        using var provider = services.BuildServiceProvider();
        var source = provider.GetRequiredService<IEfCoreDataSource<CatalogMarker>>();

        return source is EfCoreDataSource<CatalogMarker>;
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

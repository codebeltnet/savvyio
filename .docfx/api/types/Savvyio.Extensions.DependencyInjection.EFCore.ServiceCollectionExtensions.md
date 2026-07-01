---
uid: Savvyio.Extensions.DependencyInjection.EFCore.ServiceCollectionExtensions
example:
- *content
---
Composing an EF Core data layer in Savvy I/O requires three registrations: `AddEfCoreDataSource` to bind the `DbContext` and `IEfCoreDataSource`, `AddEfCoreDataStore` for generic read/write stores, and `AddEfCoreRepository` for aggregate-scoped repositories. Each registration takes the concrete type as a type argument, and the data source must be registered first because stores and repositories depend on it. The example resolves both a repository and a data store from the built service provider to confirm the bindings are correct.

```csharp
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Savvyio;
using Savvyio.Data;
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection.EFCore;
using Savvyio.Extensions.EFCore;

namespace ExampleApp;

public sealed class RegistrationExample
{
    public ServiceProvider BuildProvider()
    {
        var services = new ServiceCollection();
        services.AddEfCoreDataSource(options =>
        {
            options.ContextConfigurator = builder => builder.EnableDetailedErrors();
            options.ModelConstructor = modelBuilder => modelBuilder.Entity<Product>().HasKey(product => product.Id);
        });
        services.AddEfCoreDataStore<Product>();
        services.AddEfCoreRepository<Product, Guid>();

        return services.BuildServiceProvider();
    }

    public bool HasExpectedRegistrations(ServiceProvider provider)
    {
        var dataSource = provider.GetRequiredService<IEfCoreDataSource>();
        var dataStore = provider.GetRequiredService<IPersistentDataStore<Product, EfCoreQueryOptions<Product>>>();
        var repository = provider.GetRequiredService<IPersistentRepository<Product, Guid>>();

        return dataSource is EfCoreDataSource &&
               dataStore is EfCoreDataStore<Product> &&
               repository is EfCoreRepository<Product, Guid>;
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

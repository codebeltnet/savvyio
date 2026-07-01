---
uid: Savvyio.Extensions.DependencyInjection.EFCore.EfCoreDbContext`1
example:
- *content
---
This example shows how `EfCoreDbContext<TMarker>` can be used when a DI marker needs its own EF Core context instance.

```csharp
using System;
using Microsoft.EntityFrameworkCore;
using Savvyio;
using Savvyio.Extensions.DependencyInjection.EFCore;

namespace ExampleApp;

public sealed class MarkerContextFactory
{
    public OrdersDbContext Create()
    {
        var options = new EfCoreDataSourceOptions<OrdersMarker>
        {
            ContextConfigurator = builder => builder.EnableDetailedErrors(),
            ModelConstructor = modelBuilder => modelBuilder.Entity<OrderRecord>().HasKey(order => order.Id)
        };

        return new OrdersDbContext(options);
    }
}

public sealed class OrdersDbContext : EfCoreDbContext<OrdersMarker>
{
    public OrdersDbContext(EfCoreDataSourceOptions<OrdersMarker> options) : base(options)
    {
    }

    public DbSet<OrderRecord> Orders => Set<OrderRecord>();
}

public sealed class OrdersMarker
{
}

public sealed class OrderRecord : IIdentity<Guid>
{
    public OrderRecord(Guid id, string orderNumber)
    {
        Id = id;
        OrderNumber = orderNumber;
    }

    public Guid Id { get; }

    public string OrderNumber { get; private set; } = string.Empty;
}
```

---
uid: Savvyio.Extensions.DependencyInjection.ServiceCollectionExtensions
example:
- *content
---
Register Savvy I/O in the DI container with `AddSavvyIO`, add the handler descriptor, a service locator, configured options, a data source, and a marshaller — all through `IServiceCollection` extension methods.

```csharp
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Savvyio;
using Savvyio.Extensions.DependencyInjection;

namespace ExampleApp;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSavvyIO(options => options.EnableHandlerServicesDescriptor());
        services.AddHandlerServicesDescriptor();
        services.AddServiceLocator();
        services.AddConfiguredOptions<SavvyioOptions>(_ => { });
        services.AddDataSource<AppDataSource>();
        services.AddMarshaller<AppMarshaller>(p => new AppMarshaller());
        return services;
    }
}

public sealed class AppDataSource : IDataSource { }

public sealed class AppMarshaller : IMarshaller
{
    public Stream Serialize<TValue>(TValue value) => Stream.Null;
    public Stream Serialize(object value, System.Type inputType) => Stream.Null;
    public TValue Deserialize<TValue>(Stream data) => default!;
    public object Deserialize(Stream data, System.Type returnType) => null!;
}
```



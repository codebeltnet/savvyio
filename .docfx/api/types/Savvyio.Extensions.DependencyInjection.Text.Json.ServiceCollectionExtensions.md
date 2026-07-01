---
uid: Savvyio.Extensions.DependencyInjection.Text.Json.ServiceCollectionExtensions
example:
- *content
---
Register `JsonMarshaller` in the DI container with `AddJsonMarshaller`.

```csharp
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.Text.Json;

namespace ExampleApp;

public static class JsonDependencyInjectionSetup
{
    public static IServiceCollection AddSystemTextJsonSerialization(this IServiceCollection services)
    {
        services.AddJsonMarshaller(options =>
        {
            options.Settings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.Settings.WriteIndented = true;
        });

        return services;
    }
}
```

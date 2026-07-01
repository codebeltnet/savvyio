---
uid: Savvyio.Extensions.DependencyInjection.Newtonsoft.Json.ServiceCollectionExtensions
example:
- *content
---
Register `NewtonsoftJsonMarshaller` in the DI container with `AddNewtonsoftJsonMarshaller`.

```csharp
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Savvyio.Extensions.DependencyInjection.Newtonsoft.Json;

namespace ExampleApp;

public static class JsonDependencyInjectionSetup
{
    public static IServiceCollection AddNewtonsoftSerialization(this IServiceCollection services)
    {
        services.AddNewtonsoftJsonMarshaller(options =>
        {
            options.Settings.Formatting = Formatting.Indented;
            options.Settings.NullValueHandling = NullValueHandling.Ignore;
        });

        return services;
    }
}
```

---
uid: Savvyio.Extensions.Text.Json.JsonSerializerOptionsExtensions
example:
- *content
---
Use `Clone` to copy `JsonSerializerOptions` before applying additional serializer settings.

```csharp
using System.Text.Json;
using Savvyio.Extensions.Text.Json;
using Savvyio.Extensions.Text.Json.Converters;

namespace ExampleApp;

public static class JsonSerializerOptionsFactory
{
    public static JsonSerializerOptions CreateIndentedClone()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        options.Converters.Add(new DateTimeConverter());

        return options.Clone(copy => copy.WriteIndented = true);
    }
}
```

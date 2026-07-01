---
uid: Savvyio.Extensions.Text.Json.Converters.MetadataDictionaryConverter
example:
- *content
---
`MetadataDictionaryConverter` handles `IMetadataDictionary` serialization and deserialization, preserving the string-keyed metadata that commands, events, and requests carry through the pipeline. Register it alongside `MessageConverter` so metadata survives a full message round-trip. The example serializes a command with causation and correlation metadata and confirms the values survive deserialization.

```csharp
using System.Text.Json;
using Savvyio;
using Savvyio.Extensions.Text.Json.Converters;

namespace ExampleApp;

public static class MetadataSerialization
{
    public static IMetadataDictionary RoundTrip()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        options.Converters.Add(new MetadataDictionaryConverter());

        IMetadataDictionary metadata = new MetadataDictionary
        {
            ["tenant"] = "northwind",
            ["attempts"] = 3L
        };

        var json = JsonSerializer.Serialize(metadata, options);
        return JsonSerializer.Deserialize<IMetadataDictionary>(json, options)!;
    }
}
```

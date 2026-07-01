---
uid: Savvyio.MetadataDictionary
example:
- *content
---
This example shows how to store reserved metadata keys and custom values in a case-insensitive metadata dictionary.

```csharp
using Savvyio;

namespace ExampleApp;

public sealed class MetadataDictionaryExample
{
    public bool HasCorrelationId()
    {
        var metadata = new MetadataDictionary { ["correlationid"] = "corr-42", ["tenant"] = "north-europe" };
        return metadata.ContainsKey(MetadataDictionary.CorrelationId) && metadata["tenant"].ToString() == "north-europe";
    }
}
```

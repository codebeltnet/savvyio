---
uid: Savvyio.Extensions.Text.Json.Converters.DateTimeConverter
example:
- *content
---
Add `DateTimeConverter` to `JsonSerializerOptions` to keep `DateTime` values in ISO8601 format.

```csharp
using System;
using System.Text.Json;
using Savvyio.Extensions.Text.Json.Converters;

namespace ExampleApp;

public static class DateTimeSerialization
{
    public static SchedulingWindow RoundTrip(SchedulingWindow value)
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new DateTimeConverter());

        var json = JsonSerializer.Serialize(value, options);
        return JsonSerializer.Deserialize<SchedulingWindow>(json, options)!;
    }
}

public sealed record SchedulingWindow(DateTime StartsAt);
```

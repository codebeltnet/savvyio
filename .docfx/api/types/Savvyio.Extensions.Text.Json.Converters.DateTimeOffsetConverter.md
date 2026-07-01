---
uid: Savvyio.Extensions.Text.Json.Converters.DateTimeOffsetConverter
example:
- *content
---
Add `DateTimeOffsetConverter` to `JsonSerializerOptions` to preserve offset-aware timestamps in ISO8601 format.

```csharp
using System;
using System.Text.Json;
using Savvyio.Extensions.Text.Json.Converters;

namespace ExampleApp;

public static class DateTimeOffsetSerialization
{
    public static Appointment RoundTrip(Appointment value)
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new DateTimeOffsetConverter());

        var json = JsonSerializer.Serialize(value, options);
        return JsonSerializer.Deserialize<Appointment>(json, options)!;
    }
}

public sealed record Appointment(DateTimeOffset ScheduledAt);
```

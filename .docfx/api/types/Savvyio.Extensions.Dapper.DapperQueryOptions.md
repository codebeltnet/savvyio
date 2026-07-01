---
uid: Savvyio.Extensions.Dapper.DapperQueryOptions
example:
- *content
---
This example shows how `DapperQueryOptions` becomes a `CommandDefinition` that carries the SQL text, parameters, and timeout for one query.

```csharp
using System;
using System.Data;
using Dapper;
using Savvyio.Extensions.Dapper;

namespace ExampleApp;

public sealed class QueryOptionsExample
{
    public CommandDefinition BuildCommand()
    {
        var options = new DapperQueryOptions
        {
            CommandText = "SELECT * FROM Orders WHERE Status = @Status",
            Parameters = new { Status = "Pending" },
            CommandTimeout = TimeSpan.FromSeconds(15),
            CommandType = CommandType.Text
        };

        return options;
    }
}
```

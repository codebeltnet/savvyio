---
uid: Savvyio.MetadataExtensions
example:
- *content
---
This example shows how a request can collect reserved metadata, merge values from an upstream request, and read the normalized values back again. The workflow exercises the metadata helpers that are typically used by command, event, and integration-message pipelines.

```csharp
using System;
using Savvyio;

namespace ExampleApp;

public sealed class MetadataExtensionsExample
{
    public (string CorrelationId, string MemberType, string Tenant) Enrich()
    {
        var parent = new CreateOrderCommand("ORD-41").SetCorrelationId("corr-41").SaveMetadata("tenant", "eu-west");
        var command = new CreateOrderCommand("ORD-42")
            .SetCorrelationId("corr-42")
            .SetCausationId("checkout")
            .SetRequestId("req-42")
            .SetEventId("evt-42")
            .SetMemberType(typeof(CreateOrderCommand))
            .SetTimestamp(new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc))
            .MergeMetadata(parent);

        var correlationId = command.GetCorrelationId();
        var causationId = command.GetCausationId();
        var requestId = command.GetRequestId();
        var memberType = command.GetMemberType();
        return (correlationId, memberType, (string)command.Metadata["tenant"]);
    }
}

public sealed record CreateOrderCommand(string OrderId) : Request;
```

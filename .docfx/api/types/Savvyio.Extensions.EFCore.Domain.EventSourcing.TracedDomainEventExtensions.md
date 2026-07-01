---
uid: Savvyio.Extensions.EFCore.Domain.EventSourcing.TracedDomainEventExtensions
example:
- *content
---
`TracedDomainEventExtensions.ToByteArray` serializes an `ITracedDomainEvent` to a raw byte array using the registered marshaller, which is the format stored in the EF Core event-store table. This is called internally by the traced aggregate repository, but it can also be called directly when you need to inspect or archive event payloads. The example serializes a domain event and verifies the resulting byte array is non-empty.

```csharp
using System;
using System.IO;
using System.Text.Json;
using Savvyio;
using Savvyio.Domain.EventSourcing;
using Savvyio.Extensions.EFCore.Domain.EventSourcing;

namespace ExampleApp;

public sealed class EventPayloadExample
{
    public int CreatePayloadSize()
    {
        var domainEvent = new OrderPlaced("PO-9001");
        var payload = domainEvent.ToByteArray(new SimpleMarshaller());

        return payload.Length;
    }
}

public sealed record OrderPlaced(string OrderNumber) : TracedDomainEvent;

public sealed class SimpleMarshaller : IMarshaller
{
    public Stream Serialize<TValue>(TValue value)
    {
        return new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(value));
    }

    public Stream Serialize(object value, Type inputType)
    {
        return new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(value, inputType));
    }

    public TValue Deserialize<TValue>(Stream data)
    {
        return JsonSerializer.Deserialize<TValue>(data)!;
    }

    public object Deserialize(Stream data, Type returnType)
    {
        return JsonSerializer.Deserialize(data, returnType)!;
    }
}
```

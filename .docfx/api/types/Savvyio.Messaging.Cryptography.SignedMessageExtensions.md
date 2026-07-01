---
uid: Savvyio.Messaging.Cryptography.SignedMessageExtensions
example:
- *content
---
This example shows how to verify a signed message before a consumer trusts the request payload.

```csharp
using System;
using Savvyio;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;

namespace ExampleApp;

using System;
using System.IO;
using System.Text;
using Savvyio;

public sealed class DemoMarshaller : IMarshaller
{
    public Stream Serialize<TValue>(TValue value) => new MemoryStream(Encoding.UTF8.GetBytes(value?.ToString() ?? string.Empty));
    public Stream Serialize(object value, Type inputType) => Serialize(value?.ToString() ?? string.Empty);
    public TValue Deserialize<TValue>(Stream data) => throw new NotSupportedException();
    public object Deserialize(Stream data, Type returnType) => throw new NotSupportedException();
}

public sealed class SignedMessageExtensionsExample
{
    public void Verify()
    {
        var message = new Message<CreateOrderCommand>("msg-42", new Uri("urn:orders"), "orders.created", new CreateOrderCommand("ORD-42"));
        var signed = message.Sign(new DemoMarshaller(), options => options.SignatureSecret = new byte[] { 1, 2, 3 });
        signed.CheckSignature(new DemoMarshaller(), options => options.SignatureSecret = new byte[] { 1, 2, 3 });
    }
}

public sealed record CreateOrderCommand(string OrderId) : Request;
```

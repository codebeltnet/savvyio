---
uid: Savvyio.EventDriven.Messaging.CloudEvents.Cryptography.CloudEventExtensions
example:
- *content
---
This example shows how to sign a CloudEvent before handing it to an external bus. The sample includes a simple marshaller and verifies that the resulting signed envelope carries a non-empty signature that subscribers can later validate.

```csharp
using System;
using System.IO;
using System.Text;
using Savvyio;
using Savvyio.EventDriven;
using Savvyio.EventDriven.Messaging.CloudEvents;
using Savvyio.EventDriven.Messaging.CloudEvents.Cryptography;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;

namespace ExampleApp;

public sealed class DemoMarshaller : IMarshaller
{
    public Stream Serialize<TValue>(TValue value) => new MemoryStream(Encoding.UTF8.GetBytes(value?.ToString() ?? string.Empty));
    public Stream Serialize(object value, Type inputType) => Serialize(value?.ToString() ?? string.Empty);
    public TValue Deserialize<TValue>(Stream data) => throw new NotSupportedException();
    public object Deserialize(Stream data, Type returnType) => throw new NotSupportedException();
}

public sealed class CloudEventCryptographyExtensionsExample
{
    public bool Sign()
    {
        var message = new Message<MemberCreatedEvent>("msg-42", new Uri("https://api.example.com/members"), "members.created", new MemberCreatedEvent("MEM-42"));
        var signed = message.ToCloudEvent().SignCloudEvent(new DemoMarshaller(), options => options.SignatureSecret = new byte[] { 1, 2, 3 });
        return !string.IsNullOrWhiteSpace(signed.Signature);
    }
}

public sealed record MemberCreatedEvent(string MemberId) : Request, IIntegrationEvent;
```

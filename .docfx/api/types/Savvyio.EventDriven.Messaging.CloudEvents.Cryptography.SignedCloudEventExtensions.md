---
uid: Savvyio.EventDriven.Messaging.CloudEvents.Cryptography.SignedCloudEventExtensions
example:
- *content
---
This example shows how to verify a signed CloudEvent before deserializing it into application-level event processing.

```csharp
using System;
using Savvyio;
using Savvyio.EventDriven;
using Savvyio.EventDriven.Messaging.CloudEvents;
using Savvyio.EventDriven.Messaging.CloudEvents.Cryptography;
using Savvyio.Messaging;

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

public sealed class SignedCloudEventExtensionsExample
{
    public void Verify()
    {
        var message = new Message<MemberCreatedEvent>("msg-42", new Uri("https://api.example.com/members"), "members.created", new MemberCreatedEvent("MEM-42"));
        var signed = message.ToCloudEvent().SignCloudEvent(new DemoMarshaller(), options => options.SignatureSecret = new byte[] { 1, 2, 3 });
        signed.CheckCloudEventSignature(new DemoMarshaller(), options => options.SignatureSecret = new byte[] { 1, 2, 3 });
    }
}

public sealed record MemberCreatedEvent(string MemberId) : Request, IIntegrationEvent;
```

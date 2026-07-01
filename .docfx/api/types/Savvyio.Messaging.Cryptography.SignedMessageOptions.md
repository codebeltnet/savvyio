---
uid: Savvyio.Messaging.Cryptography.SignedMessageOptions
example:
- *content
---
This example shows how to configure the secret and algorithm used when producing verifiable signed messages.

```csharp
using Savvyio.Messaging.Cryptography;

namespace ExampleApp;

public sealed class SignedMessageOptionsExample
{
    public SignedMessageOptions Configure()
    {
        var options = new SignedMessageOptions { SignatureSecret = new byte[] { 1, 2, 3 } };
        options.ValidateOptions();
        return options;
    }
}
```

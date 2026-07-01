---
uid: Savvyio.Messaging.MessageAsyncEnumerableOptions`1
example:
- *content
---
This example shows how to configure callbacks that observe each streamed message and the properties acknowledged at the end of the sequence.

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using Savvyio;
using Savvyio.Messaging;

namespace ExampleApp;

public sealed class MessageAsyncEnumerableOptionsExample
{
    public MessageAsyncEnumerableOptions<CreateOrderCommand> Configure()
    {
        var options = new MessageAsyncEnumerableOptions<CreateOrderCommand>
        {
            MessageCallback = async message => await message.AcknowledgeAsync().ConfigureAwait(false),
            AcknowledgedPropertiesCallback = async acknowledged => await Task.CompletedTask.ConfigureAwait(false)
        };
        options.ValidateOptions();
        return options;
    }
}

public sealed record CreateOrderCommand(string OrderId) : Request;
```

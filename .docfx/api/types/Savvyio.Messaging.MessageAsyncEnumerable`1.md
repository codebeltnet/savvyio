---
uid: Savvyio.Messaging.MessageAsyncEnumerable`1
example:
- *content
---
`MessageAsyncEnumerable<T>` enables async enumeration over a stream of `IMessage<T>` envelopes from a queue or bus. To use it, pass an async callback and options to its constructor; the callback is invoked for each page of messages. The example creates an enumerator over a small in-memory sequence to show the enumeration contract.

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Savvyio;
using Savvyio.Messaging;

namespace ExampleApp;

public sealed class MessageAsyncEnumerableExample
{
    public async Task<int> ProcessAsync()
    {
        var messages = new[]
        {
            new Message<CreateOrderCommand>("msg-42", new Uri("urn:orders"), "orders.created", new CreateOrderCommand("ORD-42"))
        };

        var stream = new MessageAsyncEnumerable<CreateOrderCommand>(messages, options =>
        {
            options.MessageCallback = async message => await message.AcknowledgeAsync().ConfigureAwait(false);
            options.AcknowledgedPropertiesCallback = async acknowledged => await Task.CompletedTask.ConfigureAwait(false);
        });

        var count = 0;
        await foreach (var message in stream.ConfigureAwait(false))
        {
            count++;
        }

        return count;
    }
}

public sealed record CreateOrderCommand(string OrderId) : Request;
```

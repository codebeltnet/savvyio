---
uid: Savvyio.Commands.Messaging.InMemoryCommandQueue
example:
- *content
---
The following example shows how to enqueue command messages and drain them from `InMemoryCommandQueue` in a test-friendly workflow.

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Savvyio.Commands;
using Savvyio.Commands.Messaging;
using Savvyio.Messaging;

namespace ExampleApp.InMemoryCommandQueueSample;

public sealed class InMemoryCommandQueueUsage
{
    public InMemoryCommandQueueUsage()
    {
        RunAsync().GetAwaiter().GetResult();
    }

    public IReadOnlyList<string> ReceivedInvoiceIds => _receivedInvoiceIds;

    private readonly List<string> _receivedInvoiceIds = new();

    private async Task RunAsync()
    {
        var queue = new InMemoryCommandQueue();
        IMessage<ICommand>[] messages =
        {
            new CreateInvoiceQueueCommand("INV-42").ToMessage(new Uri("https://api.example.com/commands/invoices"), nameof(CreateInvoiceQueueCommand)),
            new CreateInvoiceQueueCommand("INV-43").ToMessage(new Uri("https://api.example.com/commands/invoices"), nameof(CreateInvoiceQueueCommand))
        };

        await queue.SendAsync(messages);

        await foreach (var message in queue.ReceiveAsync())
        {
            if (message.Data is CreateInvoiceQueueCommand command)
            {
                _receivedInvoiceIds.Add(command.InvoiceId);
            }
        }
    }
}

public sealed record CreateInvoiceQueueCommand(string InvoiceId) : Command;
```

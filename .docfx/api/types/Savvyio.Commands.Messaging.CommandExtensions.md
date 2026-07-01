---
uid: Savvyio.Commands.Messaging.CommandExtensions
example:
- *content
---
The following example shows how to wrap a command in a transport-ready `Message<T>` by calling `ToMessage`.

```csharp
using System;
using Savvyio.Commands;
using Savvyio.Commands.Messaging;
using Savvyio.Messaging;

namespace ExampleApp.CommandMessages;

public sealed class CommandExtensionsUsage
{
    public CommandExtensionsUsage()
    {
        var command = new CreateInvoiceCommandMessage("INV-42");
        IMessage<CreateInvoiceCommandMessage> message = command.ToMessage(
            new Uri("https://api.example.com/commands/invoices"),
            nameof(CreateInvoiceCommandMessage),
            options => options.MessageId = "msg-invoice-42");

        MessageId = message.Id;
        Type = message.Type;
        Source = message.Source;
    }

    public string MessageId { get; }

    public string Type { get; }

    public string Source { get; }
}

public sealed record CreateInvoiceCommandMessage(string InvoiceId) : Command;
```

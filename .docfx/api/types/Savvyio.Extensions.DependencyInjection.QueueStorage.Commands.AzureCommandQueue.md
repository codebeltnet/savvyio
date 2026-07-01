---
uid: Savvyio.Extensions.DependencyInjection.QueueStorage.Commands.AzureCommandQueue`1
example:
- *content
---
`AzureCommandQueue<TMarker>` is the DI-registered Azure Queue Storage command queue with a lifetime marker. It implements both `ISender<ICommand>` and `IReceiver<ICommand>`. Inject it directly or through one of those interfaces to send and receive commands via Azure Storage queues.

```csharp
using Savvyio.Commands;
using Savvyio.Extensions.QueueStorage.Commands;
using Savvyio.Messaging;

namespace ExampleApp;

public sealed class AzureCommandService
{
    private readonly ISender<ICommand> _sender;
    private readonly IReceiver<ICommand> _receiver;

    public AzureCommandService(AzureCommandQueue queue)
    {
        _sender = queue;
        _receiver = queue;
    }

    public ISender<ICommand> Sender => _sender;
    public IReceiver<ICommand> Receiver => _receiver;
}
```

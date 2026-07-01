---
uid: Savvyio.Extensions.DependencyInjection.SimpleQueueService.Commands.AmazonCommandQueue`1
example:
- *content
---
`AmazonCommandQueue<TMarker>` is the DI-registered Amazon SQS command queue with a lifetime marker. Inject it and use it as `IPointToPointChannel<ICommand>` to send and receive commands through Amazon SQS, or as `ISender<ICommand>` when only the send path is needed.

```csharp
using Savvyio.Commands;
using Savvyio.Extensions.SimpleQueueService.Commands;
using Savvyio.Messaging;

namespace ExampleApp;

public sealed class SqsCommandService
{
    private readonly IPointToPointChannel<ICommand> _channel;

    public SqsCommandService(AmazonCommandQueue queue)
    {
        _channel = queue;
    }

    public IPointToPointChannel<ICommand> Channel => _channel;
}
```

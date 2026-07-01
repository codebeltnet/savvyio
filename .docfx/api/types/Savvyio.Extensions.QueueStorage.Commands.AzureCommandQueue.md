---
uid: Savvyio.Extensions.QueueStorage.Commands.AzureCommandQueue
example:
- *content
---
`AzureCommandQueue` sends and receives commands through Azure Queue Storage. Configure it with `AzureQueueOptions` and an `IMarshaller` instance, then use it as `IPointToPointChannel<ICommand>`.

```csharp
using Savvyio.Commands;
using Savvyio.Extensions.QueueStorage;
using Savvyio.Extensions.QueueStorage.Commands;
using Savvyio.Messaging;

namespace ExampleApp;

public class AzureCommandQueueConfig
{
    public static AzureQueueOptions CreateOptions()
    {
        return new AzureQueueOptions
        {
            ConnectionString = "UseDevelopmentStorage=true",
            QueueName = "account-commands"
        };
    }

    public static IPointToPointChannel<ICommand> AsChannel(AzureCommandQueue queue) => queue;
}
```



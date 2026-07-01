---
uid: Savvyio.Extensions.QueueStorage.EventDriven.AzureEventBus
example:
- *content
---
`AzureEventBus` publishes and receives integration events through Azure Queue Storage. Configure it with `AzureQueueOptions` and an `IMarshaller` instance, then use it as `IPublishSubscribeChannel<IIntegrationEvent>`.

```csharp
using Savvyio.EventDriven;
using Savvyio.Extensions.QueueStorage;
using Savvyio.Extensions.QueueStorage.EventDriven;
using Savvyio.Messaging;

namespace ExampleApp;

public class AzureEventBusConfig
{
    public static AzureQueueOptions CreateOptions()
    {
        return new AzureQueueOptions
        {
            ConnectionString = "UseDevelopmentStorage=true",
            QueueName = "account-integration-events"
        };
    }

    public static IPublishSubscribeChannel<IIntegrationEvent> AsChannel(AzureEventBus bus) => bus;
}
```



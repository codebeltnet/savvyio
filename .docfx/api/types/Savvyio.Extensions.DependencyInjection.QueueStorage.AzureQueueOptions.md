---
uid: Savvyio.Extensions.DependencyInjection.QueueStorage.AzureQueueOptions`1
example:
- *content
---
`AzureQueueOptions` configured via `AddAzureCommandQueue` or `AddAzureEventBus` controls the Azure Storage connection and queue name used by the registered service.

```csharp
using Savvyio.Extensions.QueueStorage;

namespace ExampleApp;

public class DependencyInjectionQueueOptionsExample
{
    public static AzureQueueOptions CreateOptions()
    {
        return new AzureQueueOptions
        {
            ConnectionString = "UseDevelopmentStorage=true",
            QueueName = "account-commands"
        };
    }
}
```

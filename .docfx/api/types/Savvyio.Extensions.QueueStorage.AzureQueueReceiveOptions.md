---
uid: Savvyio.Extensions.QueueStorage.AzureQueueReceiveOptions
example:
- *content
---
`AzureQueueReceiveOptions` configures the receive behavior of an Azure Queue Storage consumer, including visibility timeout and message count limits. Access it through `AzureQueueOptions.ReceiveContext`.

```csharp
using System;
using Savvyio.Extensions.QueueStorage;

namespace ExampleApp;

public class AzureQueueReceiveSetup
{
    public static AzureQueueOptions CreateOptions()
    {
        var options = new AzureQueueOptions
        {
            ConnectionString = "UseDevelopmentStorage=true",
            QueueName = "account-commands"
        };
        AzureQueueReceiveOptions receiveOptions = options.ReceiveContext;
        receiveOptions.VisibilityTimeout = TimeSpan.FromMinutes(5);
        return options;
    }
}
```


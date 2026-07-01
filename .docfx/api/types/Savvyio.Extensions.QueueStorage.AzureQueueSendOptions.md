---
uid: Savvyio.Extensions.QueueStorage.AzureQueueSendOptions
example:
- *content
---
`AzureQueueSendOptions` configures message time-to-live when sending messages to Azure Queue Storage. Access it through `AzureQueueOptions.SendContext`.

```csharp
using System;
using Savvyio.Extensions.QueueStorage;

namespace ExampleApp;

public class AzureQueueSendSetup
{
    public static AzureQueueOptions CreateOptions()
    {
        var options = new AzureQueueOptions
        {
            ConnectionString = "UseDevelopmentStorage=true",
            QueueName = "account-commands"
        };
        AzureQueueSendOptions sendOptions = options.SendContext;
        sendOptions.TimeToLive = TimeSpan.FromDays(3);
        return options;
    }
}
```


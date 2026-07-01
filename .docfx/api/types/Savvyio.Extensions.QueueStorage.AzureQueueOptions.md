---
uid: Savvyio.Extensions.QueueStorage.AzureQueueOptions
example:
- *content
---
Configure an `AzureQueueOptions` with the Azure Storage account name and queue name required for Azure Queue Storage integration.

```csharp
using Savvyio.Extensions.QueueStorage;

namespace ExampleApp;

public class AzureQueueSetup
{
    public static AzureQueueOptions CreateConnectionStringOptions()
    {
        var options = new AzureQueueOptions
        {
            ConnectionString = "UseDevelopmentStorage=true",
            QueueName = "account-commands"
        };
        return options;
    }

    public static AzureQueueOptions CreateAccountOptions()
    {
        var options = new AzureQueueOptions
        {
            StorageAccountName = "mystorageaccount",
            QueueName = "account-commands"
        };
        return options;
    }
}
```

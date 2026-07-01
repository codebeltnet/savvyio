---
uid: Savvyio.Extensions.SimpleQueueService.ClientConfigExtensions
example:
- *content
---
Use `ClientConfigExtensions` to validate and access the typed AWS client configurations for SQS and SNS from a `ClientConfig[]` array.

```csharp
using Amazon.Runtime;
using Amazon.SQS;
using Savvyio.Extensions.SimpleQueueService;

namespace ExampleApp;

public class AwsClientConfigExample
{
    public static void CheckConfigurations(ClientConfig[] configurations)
    {
        if (!configurations.IsValid())
        {
            throw new System.InvalidOperationException("AWS client configurations are invalid.");
        }

        var sqsConfig = configurations.SimpleQueueService();
        var snsConfig = configurations.SimpleNotificationService();
    }
}
```

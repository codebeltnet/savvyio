---
uid: Savvyio.Extensions.QueueStorage.EventDriven.AzureEventBusOptions
example:
- *content
---
`AzureEventBusOptions` configures the Azure Event Grid topic endpoint used to publish integration events.

```csharp
using System;
using Savvyio.Extensions.QueueStorage.EventDriven;

namespace ExampleApp;

public class AzureEventBusSetup
{
    public static AzureEventBusOptions CreateOptions()
    {
        return new AzureEventBusOptions
        {
            TopicEndpoint = new Uri("https://myeventgridtopic.westeurope-1.eventgrid.azure.net/api/events")
        };
    }
}
```


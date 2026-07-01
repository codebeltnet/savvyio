---
uid: Savvyio.Extensions.DependencyInjection.QueueStorage.EventDriven.AzureEventBusOptions`1
example:
- *content
---
`AzureEventBusOptions` (or its DI generic variant) carries the Azure Event Grid topic endpoint settings used when `AddAzureEventBus` registers the event bus in DI.

```csharp
using System;
using Savvyio.Extensions.QueueStorage.EventDriven;

namespace ExampleApp;

public class DiAzureEventBusOptionsExample
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


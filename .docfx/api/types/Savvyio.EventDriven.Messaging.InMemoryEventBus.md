---
uid: Savvyio.EventDriven.Messaging.InMemoryEventBus
example:
- *content
---
`InMemoryEventBus` publishes and drains `IMessage<IIntegrationEvent>` envelopes in the same process without a real broker, making it the test-time substitute for NATS, RabbitMQ, or Azure Queue Storage. The setup requires an integration event wrapped in `IMessage<IIntegrationEvent>` via `IntegrationEventExtensions.ToMessage`; after `PublishAsync`, call `SubscribeAsync` to drain the internal queue and receive the delivered payloads. The expected outcome is that the subscriber callback fires with the original event data so you can assert message-level behavior in isolation.

```csharp
using System;
using System.Threading.Tasks;
using Savvyio.EventDriven;
using Savvyio.EventDriven.Messaging;
using Savvyio.Messaging;

namespace ExampleApp.InMemoryEventBusSample;

public sealed class InMemoryEventBusUsage
{
    public InMemoryEventBusUsage()
    {
        RunAsync().GetAwaiter().GetResult();
    }

    public string DeliveredEmailAddress { get; private set; } = string.Empty;

    private async Task RunAsync()
    {
        var bus = new InMemoryEventBus();
        IMessage<IIntegrationEvent> message = new MemberInvitedEventBusMessage("jane@example.com")
            .ToMessage(new Uri("https://api.example.com/invitations"), nameof(MemberInvitedEventBusMessage));

        await bus.PublishAsync(message);
        await bus.SubscribeAsync((received, _) =>
        {
            if (received.Data is MemberInvitedEventBusMessage invited)
            {
                DeliveredEmailAddress = invited.EmailAddress;
            }

            return Task.CompletedTask;
        });
    }
}

public sealed record MemberInvitedEventBusMessage(string EmailAddress) : IntegrationEvent;
```

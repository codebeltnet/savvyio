---
uid: Savvyio.Extensions.DependencyInjection.SimpleQueueService.EventDriven
summary: *content
---
The concrete Amazon SNS/SQS event-bus type and its options used during DI registration live in this namespace. `AmazonEventBus` publishes to SNS and receives via SQS subscriptions. `AmazonEventBusOptions` carries the topic ARN, queue URL, AWS credentials, and serialization settings.

Start with `AmazonEventBus` when you need to inspect or extend the registered implementation. Configure its options through `Savvyio.Extensions.DependencyInjection.SimpleQueueService.AddAmazonEventBus<TMarker>` rather than instantiating it directly.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

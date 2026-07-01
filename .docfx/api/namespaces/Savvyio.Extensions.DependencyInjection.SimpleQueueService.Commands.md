---
uid: Savvyio.Extensions.DependencyInjection.SimpleQueueService.Commands
summary: *content
---
The concrete Amazon SQS command-queue type and its options used during DI registration live in this namespace. `AmazonCommandQueue` sends serialized commands to an SQS queue. `AmazonCommandQueueOptions` carries the queue URL, AWS credentials, and serialization settings.

Start with `AmazonCommandQueue` when you need to inspect or extend the registered implementation. Configure its options through `Savvyio.Extensions.DependencyInjection.SimpleQueueService.AddAmazonCommandQueue<TMarker>` rather than instantiating it directly.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

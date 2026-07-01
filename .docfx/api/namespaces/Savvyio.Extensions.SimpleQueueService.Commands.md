---
uid: Savvyio.Extensions.SimpleQueueService.Commands
summary: *content
---
Savvy I/O commands delivered via Amazon SQS are handled by this namespace. `AmazonCommandQueue` implements `ICommandQueue<T>` by sending serialized commands to an SQS queue and polling to receive them. `AmazonCommandQueueOptions` configures the queue URL, AWS credentials, and serialization.

Start with `AmazonCommandQueue` as the implementation class for SQS command delivery. Choose this namespace when you need the concrete SQS command-queue type to extend, test, or configure directly; for DI-based registration, use `Savvyio.Extensions.DependencyInjection.SimpleQueueService.AddAmazonCommandQueue<TMarker>`. For the corresponding SNS event bus, see `Savvyio.Extensions.SimpleQueueService.EventDriven`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S6966:Awaitable method should be used", Justification = "No need for async in this context (small payload).", Scope = "member", Target = "~M:Savvyio.Extensions.SimpleQueueService.AmazonMessage`1.ReceiveMessagesAsync(Amazon.SQS.IAmazonSQS,System.Threading.CancellationToken)~System.Collections.Generic.IAsyncEnumerable{Savvyio.Messaging.IMessage{`0}}")]

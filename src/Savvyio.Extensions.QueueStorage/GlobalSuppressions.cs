// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S6966:Awaitable method should be used", Justification = "In this context it does not make sense.", Scope = "member", Target = "~M:Savvyio.Extensions.QueueStorage.Commands.AzureCommandQueue.SendAsync(System.Collections.Generic.IEnumerable{Savvyio.Messaging.IMessage{Savvyio.Commands.ICommand}},System.Action{Cuemon.Threading.AsyncOptions})~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Major Code Smell", "S6966:Awaitable method should be used", Justification = "In this context it does not make sense.", Scope = "member", Target = "~M:Savvyio.Extensions.QueueStorage.Commands.AzureCommandQueue.ReceiveAsync(System.Action{Cuemon.Threading.AsyncOptions})~System.Collections.Generic.IAsyncEnumerable{Savvyio.Messaging.IMessage{Savvyio.Commands.ICommand}}")]

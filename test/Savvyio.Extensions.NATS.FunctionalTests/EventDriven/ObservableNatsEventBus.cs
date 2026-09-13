using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using NATS.Client.Core;
using Savvyio.Messaging;

namespace Savvyio.Extensions.NATS.EventDriven
{
    internal sealed class ObservableNatsEventBus : NatsEventBus
    {
        private readonly object _subscribedSyncRoot = new();
        private TaskCompletionSource _subscribed = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private int _subscriptionCount;

        public ObservableNatsEventBus(IMarshaller marshaller, NatsEventBusOptions options) : base(marshaller, options)
        {
        }

        internal Task WaitUntilSubscribedAsync(int expectedSubscriptionCount = 1)
        {
            if (expectedSubscriptionCount < 1) { throw new ArgumentOutOfRangeException(nameof(expectedSubscriptionCount)); }
            return WaitUntilSubscribedCoreAsync(expectedSubscriptionCount).WaitAsync(TimeSpan.FromSeconds(10));
        }

        private async Task WaitUntilSubscribedCoreAsync(int expectedSubscriptionCount)
        {
            while (Volatile.Read(ref _subscriptionCount) < expectedSubscriptionCount)
            {
                Task subscribedTask;
                lock (_subscribedSyncRoot)
                {
                    if (_subscriptionCount >= expectedSubscriptionCount)
                    {
                        return;
                    }

                    subscribedTask = _subscribed.Task;
                }

                await subscribedTask.ConfigureAwait(false);
            }
        }

        private void SignalSubscribed()
        {
            TaskCompletionSource subscribed;
            lock (_subscribedSyncRoot)
            {
                _subscriptionCount++;
                subscribed = _subscribed;
                _subscribed = new(TaskCreationOptions.RunContinuationsAsynchronously);
            }

            subscribed.TrySetResult();
        }

        protected override async IAsyncEnumerable<ReceivedNatsMessage> SubscribeMessagesAsync(string subject, NatsSubOpts options, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var subscriptionRegistered = 0;
            var observedOptions = new NatsSubOpts
            {
                Events = new NatsSubEvents
                {
                    OnSubscribed = _ =>
                    {
                        if (Interlocked.Exchange(ref subscriptionRegistered, 1) == 0)
                        {
                            SignalSubscribed();
                        }

                        return default;
                    }
                }
            };

            await foreach (var message in base.SubscribeMessagesAsync(subject, observedOptions, cancellationToken))
            {
                yield return message;
            }
        }
    }
}

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
        private readonly TaskCompletionSource _subscribed = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public ObservableNatsEventBus(IMarshaller marshaller, NatsEventBusOptions options) : base(marshaller, options)
        {
        }

        internal Task WaitUntilSubscribedAsync()
        {
            return _subscribed.Task.WaitAsync(TimeSpan.FromSeconds(10));
        }

        protected override async IAsyncEnumerable<ReceivedNatsMessage> SubscribeMessagesAsync(string subject, NatsSubOpts options, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var observedOptions = new NatsSubOpts
            {
                Events = new NatsSubEvents
                {
                    OnSubscribed = _ =>
                    {
                        _subscribed.TrySetResult();
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

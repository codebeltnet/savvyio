using System;
using System.Threading;
using System.Threading.Tasks;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using Savvyio.Messaging;

namespace Savvyio.Extensions.NATS.Commands
{
    internal sealed class ObservableNatsCommandQueue : NatsCommandQueue
    {
        private readonly TaskCompletionSource _consumerReady = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public ObservableNatsCommandQueue(IMarshaller marshaller, NatsCommandQueueOptions options) : base(marshaller, options)
        {
        }

        internal Task WaitUntilConsumerReadyAsync()
        {
            return _consumerReady.Task.WaitAsync(TimeSpan.FromSeconds(10));
        }

        protected override async Task<INatsJSConsumer> CreateConsumerAsync(StreamConfig streamConfig, ConsumerConfig consumerConfig, CancellationToken cancellationToken)
        {
            var consumer = await base.CreateConsumerAsync(streamConfig, consumerConfig, cancellationToken).ConfigureAwait(false);
            _consumerReady.TrySetResult();
            return consumer;
        }
    }
}

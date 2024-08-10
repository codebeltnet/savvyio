using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon;

namespace Savvyio.Messaging
{
    internal class MessageAsyncEnumerator<T> : IAsyncEnumerator<IMessage<T>> where T : IRequest
    {
        private readonly IAsyncEnumerator<IMessage<T>> _source;
        private readonly IProducerConsumerCollection<IDictionary<string, object>> _acknowledgedProperties;
        private readonly Func<IEnumerable<IDictionary<string, object>>, Task> _acknowledgedPropertiesCallback;
        private readonly Func<IMessage<T>, Task> _messageCallback;

        internal MessageAsyncEnumerator(IAsyncEnumerator<IMessage<T>> source, IProducerConsumerCollection<IDictionary<string, object>> acknowledgedProperties, Func<IMessage<T>, Task> messageCallback, Func<IEnumerable<IDictionary<string, object>>, Task> acknowledgedPropertiesCallback)
        {
            Validator.ThrowIfNull(source);
            Validator.ThrowIfNull(acknowledgedProperties);
            _source = source;
            _acknowledgedProperties = acknowledgedProperties;
            _messageCallback = messageCallback;
            _acknowledgedPropertiesCallback = acknowledgedPropertiesCallback;
        }

        public IMessage<T> Current => _source.Current;

        public ValueTask DisposeAsync()
        {
            return _source.DisposeAsync();
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            var mn = await _source.MoveNextAsync().ConfigureAwait(false);
            if (mn && _messageCallback != null)
            {
                await _messageCallback(_source.Current).ConfigureAwait(false);
            }
            else
            {
                if (_acknowledgedProperties.Count > 0 && _acknowledgedPropertiesCallback != null)
                {
                    await _acknowledgedPropertiesCallback(_acknowledgedProperties).ConfigureAwait(false);
                }
            }
            return mn;
        }
    }
}

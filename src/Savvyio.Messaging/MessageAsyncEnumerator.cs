using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon;

namespace Savvyio.Messaging
{
    internal class MessageAsyncEnumerator<T> : IAsyncEnumerator<IMessage<T>> where T : IRequest
    {
        private readonly IAsyncEnumerator<IMessage<T>> _source;
        private readonly MessageAsyncEnumerableOptions<T> _options;

        internal MessageAsyncEnumerator(IAsyncEnumerator<IMessage<T>> source, MessageAsyncEnumerableOptions<T> options)
        {
            Validator.ThrowIfNull(source);
            _source = source;
            _options = options;
        }

        public IMessage<T> Current => _source.Current;

        public ValueTask DisposeAsync()
        {
            return _source.DisposeAsync();
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            var mn = await _source.MoveNextAsync().ConfigureAwait(false);
            if (mn && _options.MessageCallback != null)
            {
                _source.Current.Acknowledged += OnAcknowledgedAsync;
                await _options.MessageCallback(_source.Current).ConfigureAwait(false);
            }
            else
            {
                if (_options.AcknowledgedProperties.Count > 0 && _options.AcknowledgedPropertiesCallback != null)
                {
                    await _options.AcknowledgedPropertiesCallback(_options.AcknowledgedProperties).ConfigureAwait(false);
                }
            }
            return mn;
        }

        private Task OnAcknowledgedAsync(object sender, AcknowledgedEventArgs e)
        {
            _options.AcknowledgedProperties.TryAdd(e.Properties);
            if (sender is IAcknowledgeable message) { message.Acknowledged -= OnAcknowledgedAsync; }
            return Task.CompletedTask;
        }
    }
}

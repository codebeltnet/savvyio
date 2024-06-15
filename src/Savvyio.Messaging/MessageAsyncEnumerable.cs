using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cuemon;

namespace Savvyio.Messaging
{
    /// <summary>
    /// Exposes an enumerator that provides asynchronous iteration over values of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <seealso cref="IAsyncEnumerable{T}" />
    public class MessageAsyncEnumerable<T> : IAsyncEnumerable<IMessage<T>> where T : IRequest
    {
        private readonly IAsyncEnumerable<IMessage<T>> _source;
        private readonly MessageAsyncEnumerableOptions<T> _options;


        /// <summary>
        /// Initializes a new instance of the <see cref="MessageAsyncEnumerable{T}"/> class.
        /// </summary>
        /// <param name="source">The sequence to iterate.</param>
        /// <param name="setup">The <see cref="MessageAsyncEnumerableOptions{T}" /> which may be configured.</param>
        public MessageAsyncEnumerable(IEnumerable<IMessage<T>> source, Action<MessageAsyncEnumerableOptions<T>> setup = null) : this(source?.ToAsyncEnumerable(), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageAsyncEnumerable{T}"/> class.
        /// </summary>
        /// <param name="source">The sequence to iterate.</param>
        /// <param name="setup">The <see cref="MessageAsyncEnumerableOptions{T}" /> which may be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="setup"/> failed to configure an instance of <see cref="MessageAsyncEnumerableOptions{T}"/> in a valid state.
        /// </exception>
        public MessageAsyncEnumerable(IAsyncEnumerable<IMessage<T>> source, Action<MessageAsyncEnumerableOptions<T>> setup = null)
        {
            Validator.ThrowIfNull(source);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            _source = source;
            _options = options;
        }

        /// <summary>
        /// Returns an enumerator that iterates asynchronously through the collection.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> that may be used to cancel the asynchronous iteration.</param>
        /// <returns>An enumerator that can be used to iterate asynchronously through the collection.</returns>
        public virtual IAsyncEnumerator<IMessage<T>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new MessageAsyncEnumerator<T>(_source.GetAsyncEnumerator(cancellationToken), _options.AcknowledgedProperties, _options.MessageCallback, _options.AcknowledgedPropertiesCallback);
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Cuemon;
using Cuemon.Configuration;

namespace Savvyio.Messaging
{
    /// <summary>
    /// Configuration options that is related to <see cref="IAcknowledgeable"/> messages.
    /// </summary>
    /// <typeparam name="T">The type of the payload constraint to the <see cref="IRequest"/> interface.</typeparam>
    public class MessageAsyncEnumerableOptions<T> : IValidatableParameterObject where T : IRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageAsyncEnumerableOptions{T}"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="MessageAsyncEnumerableOptions{T}"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="AcknowledgedProperties"/></term>
        ///         <description><c>new ConcurrentBag&lt;IDictionary&lt;string, object&gt;&gt;()</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="MessageCallback"/></term>
        ///         <description><c>message =&gt; message.Acknowledged += OnAcknowledged</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public MessageAsyncEnumerableOptions()
        {
            AcknowledgedProperties = new ConcurrentBag<IDictionary<string, object>>();
            MessageCallback = message => message.Acknowledged += OnAcknowledged;
        }

        /// <summary>
        /// Gets or sets the delegate that is invoked once for each message fetched from a source.
        /// </summary>
        /// <value>The delegate that is invoked once for each message fetched from a source.</value>
        public Action<IMessage<T>> MessageCallback { get; set; }
        
        /// <summary>
        /// Gets or sets the implementation of an <see cref="IProducerConsumerCollection{T}"/> that is used to store all acknowledged properties. Default is a new instance of <see cref="ConcurrentBag{T}"/>.
        /// </summary>
        /// <value>The implementation of an <see cref="IProducerConsumerCollection{T}"/> that is used to store all acknowledged properties.</value>
        public IProducerConsumerCollection<IDictionary<string, object>> AcknowledgedProperties { get; set; }

        /// <summary>
        /// Gets or sets the delegate that is invoked at the end of a sequence with all acknowledged properties.
        /// </summary>
        /// <value>The delegate that is invoked at the end of a sequence with all acknowledged properties.</value>
        public Action<IEnumerable<IDictionary<string, object>>> AcknowledgedPropertiesCallback { get; set; }

        private void OnAcknowledged(object sender, AcknowledgedEventArgs e)
        {
            AcknowledgedProperties.TryAdd(e.Properties);
            if (sender is IAcknowledgeable message) { message.Acknowledged -= OnAcknowledged; }
        }

        /// <inheritdoc />
        public void ValidateOptions()
        {
            Validator.ThrowIfInvalidState(AcknowledgedProperties == null, $"{nameof(AcknowledgedProperties)} cannot be null.");
            Validator.ThrowIfInvalidState(MessageCallback == null, $"{nameof(MessageCallback)} cannot be null.");
        }
    }
}

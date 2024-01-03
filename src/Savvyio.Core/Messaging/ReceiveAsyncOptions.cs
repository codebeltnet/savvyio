using System;
using Cuemon.Threading;

namespace Savvyio.Messaging
{
    /// <summary>
    /// Configuration options that is related to implementations of the <see cref="IReceiver{TRequest}"/> interface.
    /// </summary>
    /// <seealso cref="AsyncOptions"/>
    public class ReceiveAsyncOptions : AsyncOptions
    {
        private int _maxNumberOfMessages;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceiveAsyncOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="AsyncOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="MaxNumberOfMessages"/></term>
        ///         <description>10</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="RemoveProcessedMessages"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public ReceiveAsyncOptions()
        {
            MaxNumberOfMessages = 10;
			RemoveProcessedMessages = true;
		}

		/// <summary>
		/// Gets or sets a value indicating whether processed messages should be removed from the broker. Default is <c>true</c>.
		/// </summary>
		/// <value><c>true</c> if processed messages should be removed from the broker; otherwise, <c>false</c>.</value>
		public bool RemoveProcessedMessages { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of messages to return.
        /// </summary>
        /// <value>The maximum number of messages to return.</value>
        /// <remarks>Max. allowed value is limited to <see cref="ushort.MaxValue"/>.</remarks>
        public int MaxNumberOfMessages
        {
            get => _maxNumberOfMessages;
            set => _maxNumberOfMessages = Math.Clamp(value, 1, ushort.MaxValue);
        }
    }
}

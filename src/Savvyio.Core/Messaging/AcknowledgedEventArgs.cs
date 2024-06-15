using System;
using System.Collections.Generic;

namespace Savvyio.Messaging
{
    /// <summary>
    /// Provides data for message related operations.
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class AcknowledgedEventArgs : EventArgs
    {
        /// <summary>
        /// Represents an event with no event data.
        /// </summary>
        public new static readonly AcknowledgedEventArgs Empty = new();

        AcknowledgedEventArgs() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AcknowledgedEventArgs"/> class.
        /// </summary>
        /// <param name="properties">The <see cref="IDictionary{TKey,TValue}"/>> to associate with the event.</param>
        public AcknowledgedEventArgs(IDictionary<string, object> properties)
        {
            Properties = properties ?? new Dictionary<string, object>();
        }

        /// <summary>
        /// Share state between components during message processing.
        /// </summary>
        /// <value>The state between components during message processing.</value>
        public IDictionary<string, object> Properties { get; }
    }
}

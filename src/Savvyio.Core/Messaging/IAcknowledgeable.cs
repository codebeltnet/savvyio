using System;
using System.Collections.Generic;

namespace Savvyio.Messaging
{
    /// <summary>
    /// Defines a generic way to make a message acknowledgeable.
    /// </summary>
    public interface IAcknowledgeable
    {
        /// <summary>
        /// Occurs when the method <see cref="Acknowledge"/> is called.
        /// </summary>
        event EventHandler<AcknowledgedEventArgs> Acknowledged;

        /// <summary>
        /// Share state between components during message processing.
        /// </summary>
        /// <value>The state between components during message processing.</value>
        IDictionary<string, object> Properties { get; }

        /// <summary>
        /// Acknowledges that this message was processed successfully.
        /// </summary>
        void Acknowledge();
    }
}

using System;
using System.Collections.Generic;

namespace Savvyio.Messaging
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IAcknowledgeable"/> interface.
    /// </summary>
    public abstract record Acknowledgeable : IAcknowledgeable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Acknowledgeable"/> class.
        /// </summary>
        protected Acknowledgeable()
        {
            Properties = new Dictionary<string, object>();
        }

        /// <inheritdoc />
        public event EventHandler<AcknowledgedEventArgs> Acknowledged;

        /// <inheritdoc />
        public IDictionary<string, object> Properties { get; }

        /// <inheritdoc />
        public void Acknowledge()
        {
            OnAcknowledged(new AcknowledgedEventArgs(Properties));
        }

        /// <summary>
        /// Raises the <see cref="Acknowledged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="AcknowledgedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnAcknowledged(AcknowledgedEventArgs e)
        {
            var handler = Acknowledged;
            handler?.Invoke(this, e);
        }
    }
}

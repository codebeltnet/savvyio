using System.Collections.Generic;
using System.Threading.Tasks;

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
        public event AsyncEventHandler<AcknowledgedEventArgs> Acknowledged;

        /// <inheritdoc />
        public IDictionary<string, object> Properties { get; }

        /// <inheritdoc />
        public async Task AcknowledgeAsync()
        {
            await OnAcknowledgedAsync(new AcknowledgedEventArgs(Properties)).ConfigureAwait(false);
        }

        /// <summary>
        /// Raises the <see cref="Acknowledged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="AcknowledgedEventArgs"/> instance containing the event data.</param>
        protected virtual async Task OnAcknowledgedAsync(AcknowledgedEventArgs e)
        {
            var handler = Acknowledged;
            if (handler != null) { await handler(this, e).ConfigureAwait(false); }
        }
    }
}

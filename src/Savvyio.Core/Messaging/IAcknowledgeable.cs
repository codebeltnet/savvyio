using System.Collections.Generic;
using System.Threading.Tasks;

namespace Savvyio.Messaging
{
    /// <summary>  
    /// Defines a generic way to make a message acknowledgeable.  
    /// </summary>  
    public interface IAcknowledgeable
    {
        /// <summary>  
        /// Occurs when the method <see cref="AcknowledgeAsync"/> is called.  
        /// </summary>  
        event AsyncEventHandler<AcknowledgedEventArgs> Acknowledged;

        /// <summary>  
        /// Share state between components during message processing.  
        /// </summary>  
        /// <value>The state between components during message processing.</value>  
        IDictionary<string, object> Properties { get; }

        /// <summary>  
        /// Acknowledges that this message was processed successfully.  
        /// </summary>  
        /// <returns>A task that represents the asynchronous operation.</returns>  
        Task AcknowledgeAsync();
    }
}

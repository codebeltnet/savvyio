using System.Collections.Generic;

namespace Savvyio
{
    /// <summary>
    /// Defines a contract about implementations of the <see cref="IHandler{TRequest}"/> interface such as name, declared members and what type of request they handle.
    /// </summary>
    public interface IHandlerServicesDescriptor
    {
        /// <summary>
        /// Generates the handler discoveries.
        /// </summary>
        /// <returns>A collection of <see cref="HandlerDiscoveryModel"/> representing the handler discoveries.</returns>
        IEnumerable<HandlerDiscoveryModel> GenerateHandlerDiscoveries();
    }
}

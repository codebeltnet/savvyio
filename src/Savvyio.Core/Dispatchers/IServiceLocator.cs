using System;
using System.Collections.Generic;

namespace Savvyio.Dispatchers
{
    /// <summary>
    /// Provides a generic way to locate implementations of service objects.
    /// </summary>
    public interface IServiceLocator
    {
        /// <summary>
        /// Get an enumeration of services of type <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>An enumeration of services of type <paramref name="serviceType"/>.</returns>
        IEnumerable<object> GetServices(Type serviceType);
    }
}

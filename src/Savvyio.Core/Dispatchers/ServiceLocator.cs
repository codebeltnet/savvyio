using System;
using System.Collections.Generic;

namespace Savvyio.Dispatchers
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IServiceLocator"/> interface.
    /// </summary>
    /// <seealso cref="IServiceLocator" />
    public class ServiceLocator : IServiceLocator
    {
        private readonly Func<Type, IEnumerable<object>> _serviceFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocator"/> class.
        /// </summary>
        /// <param name="serviceFactory">The function delegate that provides the services.</param>
        public ServiceLocator(Func<Type, IEnumerable<object>> serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        /// <summary>
        /// Get an enumeration of services of type <paramref name="serviceType" />.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>An enumeration of services of type <paramref name="serviceType" />.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _serviceFactory?.Invoke(serviceType);
        }
    }
}

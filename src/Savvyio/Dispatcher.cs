using System;
using System.Collections.Generic;
using Cuemon;

namespace Savvyio
{
    /// <summary>
    /// Represents the base class from which all implementations of the dispatcher concept should derive. This is an abstract class.
    /// </summary>
    /// <seealso cref="IDispatcher" />
    public abstract class Dispatcher : IDispatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Dispatcher"/> class.
        /// </summary>
        /// <param name="serviceFactory">The function delegate that provides the services.</param>
        protected Dispatcher(Func<Type, IEnumerable<object>> serviceFactory)
        {
            Validator.ThrowIfNull(serviceFactory, nameof(serviceFactory));
            ServiceFactory = serviceFactory;
        }

        /// <summary>
        /// Gets the function delegate that creates the services.
        /// </summary>
        /// <value>The function delegate that creates the services.</value>
        protected Func<Type, IEnumerable<object>> ServiceFactory { get;  }
    }
}

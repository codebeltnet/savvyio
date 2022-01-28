using Savvyio.Extensions.Microsoft.DependencyInjection.Storage;
using Savvyio.Extensions.Microsoft.EntityFrameworkCore;

namespace Savvyio.Extensions.Microsoft.DependencyInjection.EntityFrameworkCore
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IPersistentDataAccessObject{T,TMarker}"/> interface to support multiple implementations that serves as an abstraction layer before the actual I/O communication towards a data store using Microsoft Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="EfCoreDataAccessObject{T}" />
    /// <seealso cref="IPersistentDataAccessObject{T, TMarker}" />
    public class EfCoreDataAccessObject<T, TMarker> : EfCoreDataAccessObject<T>, IPersistentDataAccessObject<T, TMarker> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreDataAccessObject{T, TMarker}"/> class.
        /// </summary>
        /// <param name="dataStore">The <see cref="IEfCoreDataStore{TMarker}"/> that handles actual I/O communication towards a data store.</param>
        public EfCoreDataAccessObject(IEfCoreDataStore<TMarker> dataStore) : base(dataStore)
        {
        }
    }
}

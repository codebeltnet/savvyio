using Savvyio.Extensions.DependencyInjection.Data;
using Savvyio.Extensions.EFCore;

namespace Savvyio.Extensions.DependencyInjection.EFCore
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IPersistentDataAccessObject{T,TOptions,TMarker}"/> interface to support multiple implementations that serves as an abstraction layer before the actual I/O communication towards a data store using Microsoft Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="DefaultEfCoreDataAccessObject{T}" />
    /// <seealso cref="IPersistentDataAccessObject{T,TOptions,TMarker}" />
    public class DefaultEfCoreDataAccessObject<T, TMarker> : DefaultEfCoreDataAccessObject<T>, IPersistentDataAccessObject<T, EfCoreOptions<T>, TMarker> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultEfCoreDataAccessObject{T,TMarker}"/> class.
        /// </summary>
        /// <param name="dataStore">The <see cref="IEfCoreDataStore{TMarker}"/> that handles actual I/O communication towards a data store.</param>
        public DefaultEfCoreDataAccessObject(IEfCoreDataStore<TMarker> dataStore) : base(dataStore)
        {
        }
    }
}

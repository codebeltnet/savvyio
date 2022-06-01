using Savvyio.Extensions.DependencyInjection.Data;
using Savvyio.Extensions.EFCore;

namespace Savvyio.Extensions.DependencyInjection.EFCore
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IPersistentDataStore{T,TOptions,TMarker}"/> interface to support multiple implementations that serves as an abstraction layer before the actual I/O communication with a source of data using Microsoft Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="DefaultEfCoreDataStore{T}" />
    /// <seealso cref="IPersistentDataStore{T,TOptions,TMarker}" />
    public class DefaultEfCoreDataStore<T, TMarker> : DefaultEfCoreDataStore<T>, IPersistentDataStore<T, EfCoreQueryOptions<T>, TMarker> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultEfCoreDataStore{T,TMarker}"/> class.
        /// </summary>
        /// <param name="source">The <see cref="IEfCoreDataSource{TMarker}"/> that handles actual I/O communication with a source of data.</param>
        public DefaultEfCoreDataStore(IEfCoreDataSource<TMarker> source) : base(source)
        {
        }
    }
}

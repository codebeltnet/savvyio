using Savvyio.Extensions.DapperExtensions;
using Savvyio.Extensions.DependencyInjection.Dapper;
using Savvyio.Extensions.DependencyInjection.Data;

namespace Savvyio.Extensions.DependencyInjection.DapperExtensions
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IPersistentDataStore{T,TOptions,TMarker}"/> interface to support multiple implementations that is tailored for Plain Old CLR Objects (POCO) usage by DapperExtensions.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="DapperExtensionsDataStore{T}" />
    /// <seealso cref="IPersistentDataStore{T,TOptions,TMarker}" />
    public class DapperExtensionsDataStore<T, TMarker> : DapperExtensionsDataStore<T>, IPersistentDataStore<T, DapperExtensionsQueryOptions<T>, TMarker> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DapperExtensionsDataStore{T, TMarker}"/> class.
        /// </summary>
        /// <param name="source">The <see cref="IDapperDataSource{TMarker}"/> that handles actual I/O communication with a source of data.</param>
        public DapperExtensionsDataStore(IDapperDataSource<TMarker> source) : base(source)
        {
        }
    }
}

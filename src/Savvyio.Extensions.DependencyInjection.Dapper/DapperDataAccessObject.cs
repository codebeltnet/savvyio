using Savvyio.Extensions.Dapper;
using Savvyio.Extensions.DependencyInjection.Data;

namespace Savvyio.Extensions.DependencyInjection.Dapper
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IPersistentDataAccessObject{T,TOptions,TMarker}"/> interface to support multiple implementations that serves as an abstraction layer before the actual I/O communication towards a data store using Dapper.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="DapperDataAccessObject{T}"/>
    /// <seealso cref="IPersistentDataAccessObject{T,TOptions,TMarker}" />
    public class DapperDataAccessObject<T, TMarker> : DapperDataAccessObject<T>, IPersistentDataAccessObject<T, DapperOptions, TMarker> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DapperDataAccessObject{T, TMarker}"/> class.
        /// </summary>
        /// <param name="dataStore">The <see cref="IDapperDataStore{TMarker}" /> that handles actual I/O communication towards a data store.</param>
        public DapperDataAccessObject(IDapperDataStore<TMarker> dataStore) : base(dataStore)
        {
        }
    }
}

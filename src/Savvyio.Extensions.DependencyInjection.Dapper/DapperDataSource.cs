using System;
using Savvyio.Extensions.Dapper;

namespace Savvyio.Extensions.DependencyInjection.Dapper
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IDapperDataSource{TMarker}"/> interface to support the actual I/O communication with a source of data using Dapper.
    /// </summary>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data store represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="DapperDataSource" />
    /// <seealso cref="IDapperDataSource{TMarker}" />
    public class DapperDataSource<TMarker> : DapperDataSource, IDapperDataSource<TMarker>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DapperDataSource{TMarker}"/> class.
        /// </summary>
        /// <param name="options">The <see cref="DapperDataSourceOptions{TMarker}"/> used to configure this instance.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        public DapperDataSource(DapperDataSourceOptions<TMarker> options) : base(options)
        {
        }
    }
}

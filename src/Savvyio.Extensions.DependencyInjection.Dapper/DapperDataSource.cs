using System;
using Cuemon.Extensions;
using Microsoft.Extensions.Options;
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
        /// <param name="setup">The <see cref="DapperDataSourceOptions" /> which need to be configured.</param>
        public DapperDataSource(IOptions<DapperDataSourceOptions<TMarker>> setup) : base(setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DapperDataSource{TMarker}"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="DapperDataSourceOptions" /> which need to be configured.</param>
        public DapperDataSource(Action<DapperDataSourceOptions<TMarker>> setup) : this(Options.Create(setup.Configure()))
        {
        }
    }
}

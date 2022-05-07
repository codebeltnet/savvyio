using System;
using Cuemon.Extensions;
using Microsoft.Extensions.Options;
using Savvyio.Extensions.Dapper;

namespace Savvyio.Extensions.DependencyInjection.Dapper
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IDapperDataStore"/> interface to support the actual I/O communication towards a data store using Dapper.
    /// </summary>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data store represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="DapperDataStore" />
    /// <seealso cref="IDapperDataStore{TMarker}" />
    public class DapperDataStore<TMarker> : DapperDataStore, IDapperDataStore<TMarker>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DapperDataStore{TMarker}"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="DapperDataStoreOptions" /> which need to be configured.</param>
        public DapperDataStore(IOptions<DapperDataStoreOptions<TMarker>> setup) : base(setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DapperDataStore{TMarker}"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="DapperDataStoreOptions" /> which need to be configured.</param>
        public DapperDataStore(Action<DapperDataStoreOptions<TMarker>> setup) : this(Options.Create(setup.Configure()))
        {
        }
    }
}

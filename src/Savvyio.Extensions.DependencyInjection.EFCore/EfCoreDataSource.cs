using System;
using Cuemon.Extensions;
using Microsoft.Extensions.Options;
using Savvyio.Extensions.EFCore;

namespace Savvyio.Extensions.DependencyInjection.EFCore
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IEfCoreDataSource{TMarker}"/> interface to support multiple implementations that does the actual I/O communication with a source of data using Microsoft Entity Framework Core.
    /// </summary>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data store represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="EfCoreDataSource" />
    /// <seealso cref="IEfCoreDataSource{TMarker}" />
    public class EfCoreDataSource<TMarker> : EfCoreDataSource, IEfCoreDataSource<TMarker>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreDataSource{TMarker}"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="EfCoreDataSourceOptions{TMarker}" /> which need to be configured.</param>
        public EfCoreDataSource(IOptions<EfCoreDataSourceOptions<TMarker>> setup) : base(new EfCoreDbContext<TMarker>(setup))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreDataSource{TMarker}"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="EfCoreDataSourceOptions{TMarker}" /> which need to be configured.</param>
        public EfCoreDataSource(Action<EfCoreDataSourceOptions<TMarker>> setup) : this(Options.Create(setup.Configure()))
        {
        }
    }
}

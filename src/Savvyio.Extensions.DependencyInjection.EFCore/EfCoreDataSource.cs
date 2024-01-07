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
        /// <param name="options">The <see cref="EfCoreDataSourceOptions{TMarker}" /> used to configure this instance.</param>
        public EfCoreDataSource(EfCoreDataSourceOptions<TMarker> options) : base(new EfCoreDbContext<TMarker>(options))
        {
        }
    }
}

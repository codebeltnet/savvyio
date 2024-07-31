using Cuemon.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Savvyio.Extensions.EFCore;

namespace Savvyio.Extensions.DependencyInjection.EFCore
{
    /// <summary>
    /// Provides a default implementation of the <see cref="DbContext"/> class to support Savvy I/O extensions of Microsoft Entity Framework Core in multiple implementations.
    /// </summary>
    /// <typeparam name="TMarker">The type used to mark the implementation that this context represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="EfCoreDbContext" />
    /// <seealso cref="IDependencyInjectionMarker{TMarker}" />
    public class EfCoreDbContext<TMarker> : EfCoreDbContext, IDependencyInjectionMarker<TMarker>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreDbContext{TMarker}" /> class.
        /// </summary>
        /// <param name="options">The <see cref="EfCoreDataSourceOptions{TMarker}"/> used to configure this instance.</param>
        public EfCoreDbContext(EfCoreDataSourceOptions<TMarker> options) : base(options)
        {
        }
    }
}

using Cuemon.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Savvyio.Storage
{
    /// <summary>
    /// Defines a generic way to support the actual I/O communication towards a data store using Microsoft Entity Framework Core.
    /// </summary>
    /// <seealso cref="IDataStore" />
    /// <seealso cref="IUnitOfWork" />
    public interface IEfCoreDataStore : IDataStore, IUnitOfWork
    {
        /// <summary>
        /// Creates a <see cref="DbSet{TEntity}" /> that can be used to query and save instances of <typeparamref name="TEntity" />.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity for which a set should be returned.</typeparam>
        /// <returns>A set for the given entity type.</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }

    /// <summary>
    /// Defines a generic way to support multiple implementations that does the actual I/O communication towards a data store using Microsoft Entity Framework Core.
    /// </summary>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data store represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IDependencyInjectionMarker{TMarker}" />
    /// <seealso cref="IEfCoreDataStore" />
    /// <seealso cref="IUnitOfWork{TMarker}" />
    public interface IEfCoreDataStore<TMarker> : IEfCoreDataStore, IUnitOfWork<TMarker>
    {
    }
}

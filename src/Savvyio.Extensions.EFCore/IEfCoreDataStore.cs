using Microsoft.EntityFrameworkCore;
using Savvyio.Domain;

namespace Savvyio.Extensions.EFCore
{
    /// <summary>
    /// Defines a generic way to support the actual I/O communication towards a data store using Microsoft Entity Framework Core.
    /// </summary>
    /// <seealso cref="IDataStore" />
    public interface IEfCoreDataStore : IDataStore, IUnitOfWork
    {
        /// <summary>
        /// Creates a <see cref="DbSet{TEntity}" /> that can be used to query and save instances of <typeparamref name="TEntity" />.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity for which a set should be returned.</typeparam>
        /// <returns>A set for the given entity type.</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}

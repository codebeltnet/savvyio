using Microsoft.EntityFrameworkCore;
using Savvyio.Domain;

namespace Savvyio.Extensions.EFCore
{
    /// <summary>
    /// Defines a generic way to support the actual I/O communication with a source of data - tailored to Microsoft Entity Framework Core.
    /// </summary>
    /// <seealso cref="IDataSource" />
    public interface IEfCoreDataSource : IDataSource, IUnitOfWork
    {
        /// <summary>
        /// Creates a <see cref="DbSet{TEntity}" /> that can be used to query and save instances of <typeparamref name="TEntity" />.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity for which a set should be returned.</typeparam>
        /// <returns>A set for the given entity type.</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}

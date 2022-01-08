using System;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Domain
{
    /// <summary>
    /// Defines the whole of a Repository complying to the Active Record concept.
    /// </summary>
    /// <typeparam name="TAggregate">The type of the Aggregate.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies this Aggregate.</typeparam>
    public interface IActiveRecordRepository<TAggregate, in TKey> : IPersistentRepository<TAggregate>, IReadOnlyRepository<TAggregate, TKey> where TAggregate : class, IAggregateRoot<IDomainEvent, TKey>
    {
        /// <summary>
        /// Deletes an Aggregate from the specified <paramref name="id"/> asynchronous.
        /// </summary>
        /// <param name="id">The identifier of the Aggregate.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task RemoveAsync(TKey id, Action<AsyncOptions> setup = null);
        
    }
}

namespace Savvyio.Domain
{
    /// <summary>
    /// Provides a way to cover the pattern of an Aggregate Root as specified in Domain Driven Design that is optimized for Active Record. This is an abstract class.
    /// </summary>
    /// <typeparam name="TKey">The type of the key that uniquely identifies this aggregate.</typeparam>
    /// <seealso cref="Aggregate{TKey,TEvent}" />
    public abstract class AggregateRoot<TKey> : Aggregate<TKey, IDomainEvent>, IAggregateRoot<IDomainEvent, TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRoot{TKey}"/> class.
        /// </summary>
        protected AggregateRoot()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRoot{TKey}"/> class.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        protected AggregateRoot(TKey id) : base(id)
        {
        }
    }
}

namespace Savvyio.Domain
{
    /// <summary>
    /// Provides a way to cover the pattern of an Aggregate Root as specified in Domain Driven Design. This is an abstract class.
    /// </summary>
    /// <typeparam name="TKey">The type of the key that uniquely identifies this aggregate.</typeparam>
    /// <seealso cref="Aggregate{TKey,TEvent}" />
    public abstract class AggregateRoot<TKey> : Aggregate<TKey, IDomainEvent>, IAggregateRoot<IDomainEvent, TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRoot{TKey}"/> class.
        /// </summary>
        /// <param name="metadata">The optional metadata to merge with this instance.</param>
        protected AggregateRoot(IMetadata metadata = null) : base(metadata)
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

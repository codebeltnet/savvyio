namespace Savvyio.Domain
{
    /// <summary>
    /// Provides a way to cover the pattern of an Aggregate as specified in Domain Driven Design that is optimized for Active Record. This is an abstract class.
    /// </summary>
    /// <typeparam name="TKey">The type of the key that uniquely identifies this aggregate.</typeparam>
    /// <seealso cref="Aggregate{TKey,TEvent}" />
    public abstract class AggregateRoot<TKey> : Aggregate<TKey, IDomainEvent>, IAggregateRoot<TKey>
    {
        protected AggregateRoot()
        {
        }

        protected AggregateRoot(TKey id) : base(id)
        {
        }
    }
}

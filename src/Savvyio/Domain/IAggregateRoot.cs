namespace Savvyio.Domain
{
    /// <summary>
    /// Defines a marker interface for the pattern of an Aggregate as specified in Domain Driven Design.
    /// </summary>
    public interface IAggregateRoot
    {
    }

    /// <summary>
    /// Defines an Active Record capable contract for the pattern of an Aggregate as specified in Domain Driven Design.
    /// </summary>
    /// <typeparam name="TKey">The type of the key that uniquely identifies this aggregate.</typeparam>
    /// <seealso cref="IAggregateRoot" />
    /// <seealso cref="IIdentity{TKey}" />
    public interface IAggregateRoot<out TKey> : IAggregateRoot, IEntity<TKey>, IAggregateNotification<IDomainEvent>
    {
    }
}
